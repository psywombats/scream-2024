enterNVL()
enter('LEADER', 'c')
speak('LEADER', "So what'll it be, Phoebe? You know all about us and the cave now, so you can't back out now, right?")
enter('YOU', 'a')
speak('YOU', "I'll do it. I've got to do it. Thank you so much for the opportunity. I know I'm new but I'll give it everything I've got. And more.")
enter('CONTROL', 'd', 'yay')
speak('CONTROL', "I love your enthusiasm!")
expr('CONTROL', '')
enter('JERK', 'e')
speak('JERK', "Just don't overdo it.")
enter('PAL', 'b', 'grin')
speak('PAL', "Welcome aboard then.")
expr('PAL', '')
speak('PAL', "Where do you want to start, exactly, Jeanne? We can't go taking her down to the cave right away.")
speak('LEADER', "Why not? It's as good a place as any to learn the ropes.")
expr('JERK', 'meh')
speak('JERK', "She'll be safe enough in Rocklift. And Nika should have the layout modeled in Chasm, so Phoebe can see a preview.")
speak('CONTROL', "Er, Chasm's on the fritz again. I'm still debugging.")
speak('JERK', "I shouldn't have oversold it.")
expr('JERK', '')
speak('CONTROL', "No I really appreciate that you guys rely on me! I'll have it running tomorrow! Phoebe can check out the digital version of the cave then!")
speak('PAL', "Or we could head on over today and give her a taste of the real thing.")
speak('YOU', "If you think it'll be safe.")
speak('LEADER', "We're calling the top two chambers Rocklift and the Entry Chasm. We know both very well and you shouldn't be in any danger there. The light's decent as well.")
speak('YOU', "Lead the way.")
exit('LEADER')
exit('YOU')
exit('JERK')
exit('PAL')
speak('CONTROL', "And radio me when you get there! Please! Else I'll be lonely all day!")
exitNVL()

setSwitch('d1_02', true)
fade('black')
wait(1)

setSwitch('abseiling_enabled', true)
setSwitch('auto_abseil', true)
teleport('Cave01_Rocklift', 'start', 'north', true)
fade('normal')