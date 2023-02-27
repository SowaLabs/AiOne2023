import React, { useState, useEffect } from "react";

const MouthAnimation = ({ duration, mouthCues }) => {
  const [currentValue, setCurrentValue] = useState("X");

  useEffect(() => {
    let intervalId;
    const totalDuration = duration * 1000;
    setCurrentValue(mouthCues);

    const updateValue = () => {
      const currentTime = new Date().getTime() - startTime;
      const currentCue = mouthCues.find(
        (cue) => cue.start * 1000 >= currentTime
      );

      if (currentCue) {
        setCurrentValue(currentCue.value);
      } else {
        setCurrentValue("X");
      }

      if (currentTime >= totalDuration) {
        clearInterval(intervalId);
        setCurrentValue("X");
      }
    };

    const startTime = new Date().getTime();
    intervalId = setInterval(updateValue, 18);

    return () => clearInterval(intervalId);
  }, [duration, mouthCues]);

  return <div className={`mouth2 ${currentValue}`} />;
};

export default MouthAnimation;
