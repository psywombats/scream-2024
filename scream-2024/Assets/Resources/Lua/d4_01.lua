wait(.8)

enterNVL()
enter('LEADER', 'a')
enter('JERK', 'c', 'meh')
enter('CONTROL', 'd', 'grump')
speak('LEADER', "We've been waiting two hours. Phoebe, can you call Amador again?")
speak('YOU', "It just keeps ringing.")
expr('CONTROL', '')
speak('CONTROL', "I'm getting a busy signal.")
expr('JERK', 'grimace')
speak('JERK', "Because you two are calling at the same time. Face it. He's not showing.")
speak('LEADER', "I didn't expect him to be the one to sell us out...")
speak('CONTROL', "What do we do?")
expr('LEADER', 'serious')
speak('LEADER', "Look, just give me a second to think.")
exitNVL()

setSwitch('d4_01', true)