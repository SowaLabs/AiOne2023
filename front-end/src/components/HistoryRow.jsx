import React from "react";
import TextAnimation from "./TextAnimation";

const HistoryRow = ({ text, type, animate, duration, scrollToBottom }) => {
  return (
    <div className={`convo-${type}`}>
      {animate ? (
        <TextAnimation
          text={text}
          duration={duration}
          scrollToBottom={scrollToBottom}
        />
      ) : (
        text.split("\n").map((s, i) => (
          <p key={i}>
            {type === "Answer" ? "> " : ""}
            {s}
          </p>
        ))
      )}
    </div>
  );
};

export default HistoryRow;
