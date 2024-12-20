playBGM('suspense')
enterNVL()
enter('OTHER', 'c')
speak('OTHER', "Hello, me.")
speak('YOU', "You're me.")
expr('OTHER', 'flirty')
speak('OTHER', "I think it's more accurate to say that you wish you were me.")
expr('OTHER', '')
speak('YOU', "You're holding it! That's my Phoebicus. He's so cute in person. He looks exactly like I imagined.")
speak('OTHER', "Because I'm exactly what you imagined.")
expr('OTHER', 'smirk')
speak('OTHER', "I'm not just Phoebe, I'm Dr. Phoebe. You know, the one who escapes here, never looks back, and goes on to live a perfect, fulfilled life.")
expr('OTHER', '')
speak('OTHER', "I'm self-confident. I don't chase away my friends with my troubles, because I have no troubles.")
expr('OTHER', 'flirty')
speak('OTHER', "I'm well-adjusted. I love myself. And everyone else loves me too.")
speak('YOU', "Then... you're... not me.")
expr('OTHER', '')
speak('OTHER', "Not as you are now. Is it really such a big deal?")
expr('OTHER', 'smirk')
speak('OTHER', "Don't you hate you? Isn't that why you're here? Because you'd rather be me than you?")
expr('OTHER', '')
speak('YOU', "That... I...")
exit('OTHER')
enter('OTHER', 'd')
enter('YOU', 'b')
speak('YOU', "I don't feel like I've earned it. Are you just here to grant all my wishes? Just like that?")
speak('OTHER', "Of course you've earned it. Well, I earned it. I journeyed all the way here to heart of the cave. Everything I have, I've worked for.")
expr('OTHER', 'smirk')
speak('OTHER', "Now that you're here, all that's left for me to do is return to the surface and live out my life.")
expr('OTHER', '')
speak('YOU', "What about me?")
speak('OTHER', "Are you me or not?")
speak('OTHER', "You just need to shake my hand and admit that I am far more 'you' than you will ever be without 'me'.")
expr('OTHER', 'flirty')
speak('OTHER', "Do we understand each other?")
exit('YOU')
exit('OTHER')
enter('OTHER', 'c')
speak('OTHER', "Look deep within. Do you see your self? Or do you see ME?")
speak('YOU', "I...")
wait(.5)
exitNVL()

fade('black', 4)
intertitle("DAY 0x99", "if then")
wait(1)
setString('date', '')
setString('time', '')
setSwitch('d5_05', true)
teleport('Clubhouse', 'start', 'NORTH', true)
fade('normal', 2)
