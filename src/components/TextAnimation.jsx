import React, { useState, useEffect } from "react";

const TextAnimation = ({ text, duration, scrollToBottom }) => {
  const [sentences, setSentences] = useState([]);
  const [jsx, setJsx] = useState(<p></p>);

  useEffect(() => {
    // Split the text into sentences
    setSentences(text.split(/\r?\n/).map((sentence) => sentence.split(" ")));
  }, [text]);

  useEffect(() => {
    const totalTokens =
      sentences.reduce((acc, s) => acc + s.length, 0) + sentences.length;
    const intervalTime = duration / totalTokens;
    let i = 0;
    const interval = setInterval(() => {
      const newSentances = [];
      const sentancesLength = sentences.map((s) => s.length);
      for (let x = 0; x < sentences.length; x++) {
        const totalSentanceLengthBefore = sentancesLength
          .slice(0, x)
          .reduce((acc, s) => acc + s, 0);
        if (totalSentanceLengthBefore > i) break;
        let tempSentence = [];
        for (let y = 0; y < sentences[x].length; y++) {
          if (totalSentanceLengthBefore + y > i) break;
          tempSentence.push(sentences[x][y]);
        }
        newSentances.push(tempSentence);
      }
      setJsx(
        <>
          {newSentances.map((s, i) => (
            <p key={i}>
              {"> "}
              {s.map((w, wi) => (
                <span key={wi}>{w} </span>
              ))}
            </p>
          ))}
        </>
      );
      scrollToBottom();
      i++;
      if (i >= totalTokens) {
        clearInterval(interval);
      }
    }, intervalTime * 1000);
    return () => clearInterval(interval);
  }, [sentences, duration, scrollToBottom]);

  return jsx;
};

export default TextAnimation;
