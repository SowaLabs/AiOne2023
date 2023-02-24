import { useEffect, useState, useRef } from "react";
import "./App.css";
import HistoryRow from "./components/HistoryRow";
import MouthAnimation from "./components/MouthAnimation";
import ellipsis from "./ellipsis.svg";

const fetchResponse = async (question) => {
  try {
    const response = await fetch(
      `https://chatbot.oddbit-retro.org/chatbot?${new URLSearchParams({
        question,
      })}`
    );
    const data = await response.json();
    return {
      type: "Answer",
      text: data.answerText,
      audio: data.answerSpeechAudioFile,
      //data.lipSyncAnimation.mouthCues,
      mouthCues: [
        { start: 0.0, end: 0.02, value: "X" },
        { start: 0.02, end: 0.0, value: "C" },
        { start: 0.09, end: 0.02, value: "B" },
        { start: 0.16, end: 0.09, value: "E" },
        { start: 0.23, end: 0.16, value: "A" },
        { start: 0.31, end: 0.23, value: "E" },
        { start: 0.41, end: 0.31, value: "A" },
        { start: 0.53, end: 0.41, value: "E" },
        { start: 0.66, end: 0.53, value: "F" },
        { start: 0.73, end: 0.66, value: "C" },
        { start: 0.94, end: 0.73, value: "E" },
        { start: 1.01, end: 0.94, value: "C" },
        { start: 1.15, end: 1.01, value: "B" },
        { start: 1.22, end: 1.15, value: "X" },
        { start: 1.61, end: 1.22, value: "F" },
        { start: 1.95, end: 1.61, value: "B" },
        { start: 2.09, end: 1.95, value: "C" },
        { start: 2.23, end: 2.09, value: "B" },
        { start: 2.58, end: 2.23, value: "G" },
        { start: 2.65, end: 2.58, value: "A" },
        { start: 2.73, end: 2.65, value: "E" },
        { start: 2.91, end: 2.73, value: "B" },
        { start: 3.12, end: 2.91, value: "C" },
        { start: 3.33, end: 3.12, value: "A" },
        { start: 3.42, end: 3.33, value: "C" },
        { start: 3.64, end: 3.42, value: "B" },
        { start: 3.78, end: 3.64, value: "C" },
        { start: 3.85, end: 3.78, value: "H" },
        { start: 3.92, end: 3.85, value: "E" },
        { start: 4.0, end: 3.92, value: "F" },
        { start: 4.2, end: 4.0, value: "B" },
        { start: 4.38, end: 4.2, value: "C" },
        { start: 4.42, end: 4.38, value: "A" },
        { start: 4.5, end: 4.42, value: "F" },
        { start: 4.64, end: 4.5, value: "B" },
        { start: 4.85, end: 4.64, value: "A" },
        { start: 4.95, end: 4.85, value: "D" },
        { start: 5.01, end: 4.95, value: "A" },
        { start: 5.09, end: 5.01, value: "C" },
        { start: 5.16, end: 5.09, value: "B" },
        { start: 5.35, end: 5.16, value: "E" },
        { start: 5.42, end: 5.35, value: "C" },
        { start: 5.48, end: 5.42, value: "B" },
        { start: 5.62, end: 5.48, value: "C" },
        { start: 5.69, end: 5.62, value: "A" },
        { start: 5.83, end: 5.69, value: "X" },
        { start: 6.51, end: 5.83, value: "B" },
        { start: 6.58, end: 6.51, value: "F" },
        { start: 7.13, end: 6.58, value: "A" },
        { start: 7.22, end: 7.13, value: "B" },
        { start: 7.65, end: 7.22, value: "C" },
        { start: 7.93, end: 7.65, value: "B" },
        { start: 8.07, end: 7.93, value: "C" },
        { start: 8.14, end: 8.07, value: "A" },
        { start: 8.22, end: 8.14, value: "B" },
        { start: 8.33, end: 8.22, value: "F" },
        { start: 8.54, end: 8.33, value: "B" },
        { start: 8.87, end: 8.54, value: "F" },
        { start: 9.06, end: 8.87, value: "B" },
        { start: 9.13, end: 9.06, value: "E" },
        { start: 9.41, end: 9.13, value: "B" },
        { start: 9.62, end: 9.41, value: "X" },
        { start: 9.82, end: 9.62, value: "B" },
        { start: 10.1, end: 9.82, value: "F" },
        { start: 10.1, end: 10.1, value: "G" },
        { start: 10.2, end: 10.1, value: "F" },
        { start: 10.3, end: 10.2, value: "B" },
        { start: 10.5, end: 10.3, value: "X" },
        { start: 11.1, end: 10.5, value: "B" },
        { start: 11.2, end: 11.1, value: "F" },
        { start: 11.2, end: 11.2, value: "A" },
        { start: 11.3, end: 11.2, value: "F" },
        { start: 11.5, end: 11.3, value: "C" },
        { start: 11.7, end: 11.5, value: "B" },
        { start: 11.8, end: 11.7, value: "A" },
        { start: 11.9, end: 11.8, value: "C" },
        { start: 12.0, end: 11.9, value: "F" },
        { start: 12.1, end: 12.0, value: "A" },
        { start: 12.2, end: 12.1, value: "C" },
        { start: 12.2, end: 12.2, value: "A" },
        { start: 12.3, end: 12.2, value: "C" },
        { start: 12.4, end: 12.3, value: "B" },
        { start: 12.5, end: 12.4, value: "A" },
        { start: 12.6, end: 12.5, value: "B" },
        { start: 12.6, end: 12.6, value: "A" },
        { start: 12.7, end: 12.6, value: "C" },
        { start: 12.8, end: 12.7, value: "F" },
        { start: 12.9, end: 12.8, value: "B" },
        { start: 13.1, end: 12.9, value: "X" },
        { start: 13.4, end: 13.1, value: "B" },
        { start: 13.5, end: 13.4, value: "F" },
        { start: 13.6, end: 13.5, value: "A" },
        { start: 13.7, end: 13.6, value: "C" },
        { start: 13.8, end: 13.7, value: "B" },
        { start: 13.9, end: 13.8, value: "F" },
        { start: 14.1, end: 13.9, value: "B" },
        { start: 14.3, end: 14.1, value: "E" },
        { start: 14.4, end: 14.3, value: "B" },
        { start: 14.6, end: 14.4, value: "A" },
        { start: 14.7, end: 14.6, value: "C" },
        { start: 14.8, end: 14.7, value: "E" },
        { start: 14.9, end: 14.8, value: "C" },
        { start: 15.1, end: 14.9, value: "B" },
        { start: 15.4, end: 15.1, value: "A" },
        { start: 15.4, end: 15.4, value: "B" },
        { start: 15.6, end: 15.4, value: "C" },
        { start: 15.7, end: 15.6, value: "B" },
        { start: 15.8, end: 15.7, value: "C" },
        { start: 15.9, end: 15.8, value: "B" },
        { start: 16.0, end: 15.9, value: "C" },
        { start: 16.1, end: 16.0, value: "B" },
        { start: 16.4, end: 16.1, value: "X" },
        { start: 17.1, end: 17.13, value: "X" },
      ],
      duration: 17.3, //data.lipSyncAnimation.duration,
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
    const answer = await fetchResponse(newQuestion);
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

  const scrollToBottom = () => {
    historyChatRef.current?.scrollIntoView({ behavior: "smooth" });
  };

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
