enterNVL()
enter('LEADER', 'a')
enter('CONTROL', 'c')
enter('YOU', 'd')
enter('JERK', 'e')
speak('LEADER', "Alright, it's past lunch and still no sign of Amador. Let's bring it in.")
speak('JERK', "What do you want to do, Jeanne?")
speak('LEADER', "If we can't get in touch with Amador via cell or by radio, then he must be in the cave. He must be past the camp, in a place where the radio can't reach.")
speak('LEADER', "We should go in after him. We can be the search and rescue team.")
expr('JERK', 'grimace')
speak('JERK', "Do you actually think Amador went down there on his own, or do you just need an excuse to go back to the cave without him?")
expr('LEADER', 'serious')
speak('LEADER', "Are you accusing me of something?")
expr('JERK', '')
speak('LEADER', "I just want to find him as soon as possible. Who's with me?")
exit('CONTROL')
enter('CONTROL', 'b')
speak('CONTROL', "The simulations agree he must be in an unexplored chamber. I vote yes.")
speak('YOU', "I don't think my vote even counts.")
expr('JERK', 'meh')
speak('JERK', "I'll come with you, Jeanne. But if you ...did... anything to Amador, I'm breaking off the agreement and I'm going to the cops.")
speak('LEADER', "While it doesn't make me happy to hear you say that, I'm glad we're all on board.")
expr('LEADER', 'grin')
speak('LEADER', "We stick to the radio relay plan. We'll haul the repeater down as far as we can, and that should let us stay in touch with Nika even in the depths.")
speak('LEADER', "And from there, we search for Amador. Any questions?")
speak('LEADER', "Then let's set off.")
exitNVL()

fade('black', 2)
teleport('Cave07_Canal2', 'start')