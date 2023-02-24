import React, { useState, useEffect } from "react";

const TextAnimation = ({ text, duration }) => {
  const [words, setWords] = useState([]);
  const [currentWordIndex, setCurrentWordIndex] = useState(0);

  useEffect(() => {
    // Split the input text into an array of words
    const wordsArray = text.split(" ");
    setWords(wordsArray);

    // Calculate the duration of each word based on the total duration
    const wordDuration = duration / wordsArray.length;

    // Loop through each word and schedule its animation
    let i = 0;
    const intervalId = setInterval(() => {
      if (i === wordsArray.length) {
        clearInterval(intervalId);
        return;
      }

      setCurrentWordIndex(i);
      i++;
    }, wordDuration * 1000);

    return () => {
      clearInterval(intervalId);
    };
  }, [text, duration]);

  return (
    <>
      {words
        .filter((w, i) => i <= currentWordIndex)
        .map((word, index) => (
          <span
            key={index}
            style={{
              fontWeight: index === currentWordIndex ? "bold" : "normal",
            }}
          >
            {word}
            {index !== words.length - 1 ? " " : ""}
          </span>
        ))}
    </>
  );
};

export default TextAnimation;
