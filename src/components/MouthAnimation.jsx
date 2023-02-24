import React, { useState, useEffect } from "react";

const MouthAnimation = ({ duration, mouthCues }) => {
  const [currentValue, setCurrentValue] = useState("");

  useEffect(() => {
    let intervalId;
    const totalDuration = duration * 1000;

    const updateValue = () => {
      const currentTime = (new Date().getTime() - startTime) / 1000;
      //const progress = Math.min(currentTime / totalDuration, 1);
      const currentCue = mouthCues.find(
        (cue) => cue.start >= currentTime && cue.end <= currentTime
      );

      console.log(currentTime, mouthCues, currentCue);

      if (currentCue) {
        setCurrentValue(currentCue.value);
      } else {
        setCurrentValue("X");
      }

      if (currentTime >= totalDuration / 1000) {
        clearInterval(intervalId);
        setCurrentValue("");
      }
    };

    const startTime = new Date().getTime();
    intervalId = setInterval(updateValue, 50);

    return () => clearInterval(intervalId);
  }, [duration, mouthCues]);

  return <div className={`mouth2 ${currentValue}`} />;
};

export default MouthAnimation;
