enterNVL()
enter('PAL', 'c')
speak('PAL', "I'm heading out. See you all tomorrow?")
enter('LEADER', 'e')
speak('LEADER', "Looking forward to it.")
exit('LEADER')
enter('CONTROL', 'e')
speak('CONTROL', "Seeya Amador.")
exit('CONTROL')
speak('PAL', "Oh, and Phoebe...")
enter('YOU', 'b')
speak('YOU', "Everything alright?")
expr('PAL', 'dis')
speak('PAL', "...")
expr('PAL', 'huh')
speak('PAL', "Yeah, it's fine. Sorry, I just had a crazy intrusive thought.")
expr('PAL', '')
speak('PAL', "I'll see you at the bottom.")
exit('PAL')
speak('YOU', "...")
exitNVL()

setSwitch('d3_05', true)
fade('black', 2)
intertitle("DAY 0x04", "virtual")
teleport('DreamChamber', 'start')