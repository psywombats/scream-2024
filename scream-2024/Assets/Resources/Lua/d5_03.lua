radio('LEADER', " -- anyone?")
radio('YOU', "Jeanne!? You're alive! Jeanne can you hear me? Are you hurt?")
radio('LEADER', " -- roger. Hi Phoebe. My ankle's swelling but I'll be alright. I'm -- broke 2000 meters, I -- ")
radio('YOU', "I thought you were dead. Tom ditched us. Where are you? ")
radio('LEADER', " -- at the bottom. Waiting. We're here, me and -- ")
radio('YOU', "The bottom?")
radio('LEADER', " -- truly at peace.")
radio('YOU', "Jeanne, are you still there?")
exitRadio()

enterNVL()
enter('YOU', 'c')
speak('YOU', "I need to get to the bottom. I NEED to.")
exitNVL()

setSwitch('d5_03', true)