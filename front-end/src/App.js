import { useEffect, useState, useRef, useCallback } from "react";
import "./App.css";
import HistoryRow from "./components/HistoryRow";
import MouthAnimation from "./components/MouthAnimation";
import ellipsis from "./ellipsis.svg";

const uuidv4 = () =>
  ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
    (
      c ^
      (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
    ).toString(16)
  );

const fetchResponse = async (question, uuid) => {
  try {
    const response = await fetch(
      `https://chatbot.oddbit-retro.org/chatbot?${new URLSearchParams({
        question,
        sessionId: uuid,
      })}`
    );
    const data = await response.json();
    return {
      type: "Answer",
      text: data.answerText,
      audio: data.answerSpeechAudioFile,
      mouthCues: data.lipSyncAnimation.mouthCues,
      duration: data.lipSyncAnimation.duration,
    };
  } catch (e) {
    return {
      type: "Error",
      text: e.toString(),
      duration: 2000,
      mouthCues: [
        { start: 0.0, end: 0.05, value: "X" },
        { start: 0.05, end: 0.27, value: "D" },
        { start: 0.27, end: 0.31, value: "C" },
        { start: 0.31, end: 0.43, value: "B" },
        { start: 0.43, end: 0.47, value: "X" },
      ],
    };
  }
};

const playAudio = (base64WavString) => {
  const decodedWav = atob(base64WavString);
  const buffer = new Uint8Array(decodedWav.length);
  for (let i = 0; i < decodedWav.length; i++) {
    buffer[i] = decodedWav.charCodeAt(i);
  }
  const blob = new Blob([buffer], { type: "audio/wav" });
  const url = URL.createObjectURL(blob);
  const audio = new Audio(url);
  audio.play();
};

const App = () => {
  const [query, setQuery] = useState("");
  const [history, setHistory] = useState([]);
  const [isTyping, setIsTyping] = useState(false);
  const [isAnswering, setIsAnswering] = useState(false);
  const [duration, setDuration] = useState(0);
  const [sessionId] = useState(uuidv4());
  const [mouthCues, setMouthCues] = useState([]);
  const historyChatRef = useRef(null);
  const inputRef = useRef(null);
  const debouncedIsTyping = useDebounce(isTyping, 200);

  useEffect(() => {
    if (inputRef.current !== null) {
      inputRef.current.focus();
    }
  }, [isAnswering]);

  const handleSubmit = () => {
    if (query === "") return;
    const newQuestion = "" + query;
    const newHistory = [...history, { text: newQuestion, type: "User" }];
    setHistory(newHistory);
    setQuery("");
    setIsTyping(false);
    setTimeout(scrollToBottom);
    handleAnswering(newQuestion, newHistory);
  };

  const handleAnswering = async (newQuestion, newHistory) => {
    setIsAnswering(true);
    const answer = await fetchResponse(newQuestion, sessionId);
    if (answer.audio) {
      playAudio(answer.audio);
    }
    setDuration(answer.duration || 0);
    setMouthCues(answer.mouthCues || []);
    setHistory([...newHistory, { ...answer, audioDuration: answer.duration }]);
    setIsAnswering(false);
    setTimeout(scrollToBottom);
  };

  const handleTyping = (input) => {
    if (input !== "") {
      setIsTyping(true);
    } else {
      setIsTyping(false);
    }
    setQuery(input);
  };

  const scrollToBottom = useCallback(() => {
    historyChatRef.current?.scrollIntoView({ behavior: "smooth" });
  }, []);

  return (
    <div className="container">
      <div className="image-container">
        <div
          className={`image mascot ${debouncedIsTyping ? "pose1" : "normal"}`}
        >
          <MouthAnimation duration={duration} mouthCues={mouthCues} />
        </div>
      </div>
      <div className="input-box">
        <div className="convo-history">
          {history.map((item, index) => {
            const text = item.text;
            const type = item.type;
            const duration = item.duration;
            const shouldAnimate =
              type !== "User" && index === history.length - 1;
            return (
              <HistoryRow
                key={index}
                text={text}
                type={type}
                duration={duration}
                animate={shouldAnimate}
                scrollToBottom={scrollToBottom}
              />
            );
          })}
          {isAnswering && (
            <img className="ellipsis" src={ellipsis} alt="loading anwser" />
          )}
          <div className="history-end" ref={historyChatRef} />
        </div>
        <input
          ref={inputRef}
          value={query}
          type="text"
          placeholder="Talk to me :)"
          disabled={isAnswering}
          onChange={(event) => handleTyping(event.target.value)}
          onKeyDown={(e) => {
            if (e.key === "Enter") {
              handleSubmit();
            }
          }}
        />
      </div>
    </div>
  );
};

// Hook
const useDebounce = (value, delay) => {
  // State and setters for debounced value
  const [debouncedValue, setDebouncedValue] = useState(value);
  useEffect(
    () => {
      // Update debounced value after delay
      const handler = setTimeout(() => {
        setDebouncedValue(value);
      }, delay);
      // Cancel the timeout if value changes (also on delay change or unmount)
      // This is how we prevent debounced value from updating if value is changed ...
      // .. within the delay period. Timeout gets cleared and restarted.
      return () => {
        clearTimeout(handler);
      };
    },
    [value, delay] // Only re-call effect if value or delay changes
  );
  return debouncedValue;
};

export default App;
