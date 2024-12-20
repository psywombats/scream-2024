enterNVL()
enter('YOU', 'c')
speak('YOU', "Oh shi -")
exitNVL()

playBGM('unforgettable_exp')
wait(.6)

radio('YOU', "Guys! Get over here! Quick! Amador! Jeanne!")
radio('PAL', "-- Phoebe? Where --", 'bad')
radio('YOU', "Damn radio.")
radio('YOU', "Amador! Come in! There's someone... here.")
radio('LEADER', "We can't hear -- ", 'bad')
radio('JERK', "I'm on my way.", 'okay')
exitRadio()

wait(2)

enterNVL()
enter('YOU', 'e')
enter('JERK', 'c')
speak('YOU', "Oh thank god.")
speak('JERK', "...")
enter('LEADER', 'b', 'serious')
speak('LEADER', "So we're not the first ones here.")
enter('PAL', 'a')
speak('PAL', "Yeesh, glad that's not me.")
speak('JERK', "Check your air sensors.")
speak('PAL', "All good here.")
speak('YOU', "Is... is he... fresh?")
exit('JERK')
enter('JERK', 'd')
speak('JERK', "Carbide lamp... Hemp rope... That's vintage gear. He's been down here three, maybe four decades.")
speak('PAL', "Maybe he was a miner...")
exit('JERK')
enter('JERK', 'c', 'meh')
speak('JERK', "He has no industrial equipment. We can't rule out that he was here caving just like us, either.")
expr('JERK', '')
speak('YOU', "What do we do?")
speak('LEADER', "We've set up our camp and Phoebe found a new branch. Those were our goals for today. We start the ascent back out, now.")
speak('YOU', "Right behind you.")
exitNVL()

setSwitch('d2_03', true)

teleport('Clubhouse', 'd2_04')