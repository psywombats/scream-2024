enterNVL()
enter('YOU', 'c')
speak('YOU', "Jeanne! There you are!")
speak('YOU', "I've been trying to reach you via radio, but you kept cutting out. Are you hurt? Why aren't you -")
exit('YOU')
enter('YOU', 'b')
playBGM('unforgettable_exp')
speak('YOU', "Dead!")
exit('YOU')
enter('YOU', 'e')
speak('YOU', "Legs do not bend like that legs do not bend like that legs do not bend like - ")
exit('YOU')
enter('YOU', 'b')
speak('YOU', "Oh god, what happened to you? You poor thing...")
speak('YOU', "If you died in that fall, how did you get all the way over here?")
speak('YOU', "Did you fall again after the slip in the sump? Did you reach the bottom and come back for me?")
speak('YOU', "Is this why you stopped responding on the radio? Or...")
exitNVL()

radio('CONTROLD', "Hellooo? Anyone read me?", 'okay')
radio('YOU', "Nika! It's Phoebe! Everyting's falling apart down here.")
radio('CONTROLD', "I figured. That's why I decided to come in person. Tom and Jeanne ditched you, huh?")
radio('YOU', "Jeanne is dead. I found her body. But even after the fall that I think killed her, she was talking to me on the radio?")
radio('YOU', "Wait, how am I talking to you right now?")
radio('CONTROLD', "Like I said, I decided I'd come in person. You're entirely within Chasm's simulation right now, so I figure I'd just patch myself in.")
radio('YOU', "But the repeater's range shouldn't...")
radio('CONTROLD', "Nah, I'm close enough to radio you normally. I think I'm about 1800 meters down? Don't worry, I'll be there soon.")
radio('CONTROLD', "And don't worry about Jeanne's body. That was just her from the previous meta level. Once you reach the bottom, you'll be able to follow her out of the simulation too.")
radio('YOU', "Nika, where are you? I'm going to head your way, okay?")
radio('CONTROLD', "Just head to the bottom and we'll chat there. Sound good?")
radio('YOU', "No!")
radio('CONTROLD', "Looks, there's some pretty technical abseiling here, so I've got to jump off for now. See you soon!")
exitRadio()

enterNVL()
enter('YOU', 'c')
speak('YOU', "You absolutely insane person. How is she even...")
speak('YOU', "Never mind. The bottom. Focus.")
exitNVL()

setSwitch('d5_04', true)
teleport('Cave13_Squeeze', 'start')