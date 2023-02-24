import React from "react";
import TextAnimation from "./TextAnimation";

const HistoryRow = ({ text, type, animate, duration }) => {
  return (
    <p className={`convo-${type}`}>
      {type}:{" "}
      {animate ? <TextAnimation text={text} duration={duration} /> : text}
    </p>
  );
};

export default HistoryRow;
