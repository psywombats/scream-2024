enterNVL()
enter('PAL', 'c')
speak('PAL', "Welcome to my humble abode.")
enter('YOU', 'e')
speak('YOU', "You sleep here?")
speak('PAL', "Beats paying rent. Off-campus apartments cost a ton.")
speak('YOU', "Never mind. I didn't know you were in any clubs. What exactly goes on in here?")
speak('PAL', "Oh, nothing. This is just the meeting space. All the good stuff happens in the caves.")
speak('YOU', "Caves?")
speak('PAL', "'Course. Welcome to the A&M University Caving Club!")
exit('PAL')

enter('JERK', 'b')
speak('JERK', "Technically it's the Speleological Society.")
enter('LEADER', 'c')
speak('LEADER', "Does it really matter, Tom? So long as people are interested in joining, I don't care what they call it.")
speak('LEADER', "Hi Amador. And you're... Phoebe?")
speak('YOU', "Hi, yep, that's me. I'm a biology PhD candidate, specializing in reptiles and herpetology, but it's been five years and the chances keep getting slimmer, and ugh, I'm talking too much, and I totally forgot your name.")
speak('LEADER', "Probably because I didn't mention it. I'm Jeanne, and while we run by committee here, I guess I'm technically in charge.")
speak('JERK', "You're not in charge. You just head the committee.")
enter('CONTROL', 'a')
speak('CONTROL', "It's fine! There aren't enough of us to be too formal. I'll call Jeanne the leader just because she handles all the paperwork with the university and I've got way better things to do than deal with that.")
enter('PAL', 'd')
speak('PAL', "Like exploring new caves.")
speak('LEADER', "Exactly!")
speak('LEADER', "Although, uh, Amador, I'd really appreciate if you didn't go spreading the word around about the discovery, as much as it sounds like Phoebe is a good fit for us...")
speak('PAL', "Phoebe's a pal, Jeanne. She's not going to go spilling the beans on you.")

exit('PAL')
exit('LEADER')
exit('CONTROL')
exit('JERK')
enter('PAL', 'b')
speak('PAL', "Basically, Phoebe, we found an unexplored cave connected to one of the old mines on campus.")
speak('PAL', "It's deep. Real deep.")
enter('CONTROL', 'a')
speak('CONTROL', "Not on any map, either!")
exit('CONTROL')
enter('JERK', 'a')
speak('JERK', "Which makes it dangerous.")
exit('JERK')
enter('LEADER', 'a')
speak('LEADER', "But there's no reward without risk.")
speak('LEADER', "This thing goes down a long, long ways. Deeper than anything else in the region, or even the state.")
speak('LEADER', "We're still trying to map it out, but we're a bit short-handed.")
speak('YOU', "It sounds, um... Like a great opportunity? But I'm not sure how - ")
speak('PAL', "That whole lagoon trip you went on -- that was to find a new frog subspecies, right?")
speak('YOU', "Really any amphibian would do. I'm trying to study evolutionary divergence in the Sauropsida clade, so - ")
exit('YOU')
enter('YOU', 'd')
speak('YOU', "Wait, you think this cave of yours could have water in it? Maybe even reptiles?")
speak('LEADER', "It goes at least down to the water table.")
exit('YOU')
enter('YOU', 'e')
speak('YOU', "If you found mudpuppies this far west, or salamanders, or maybe a new species of olm, or...")
speak('YOU', "A new species would be a dream come true. Like, career-launching. That'd blow Dr. Mehta's head off. You think this could actually be a thing? In your cave?")
speak('LEADER', "Only one way to find out.")
exit('LEADER')
enter('CONTROL', 'a')
speak('CONTROL', "Only three of us can actually explore right now, so, it's been hard going, but...")
enter('JERK', 'c')
speak('JERK', "We could use another set of hands. Competent hands.")
speak('YOU', "I really don't know... This sounds exciting, but, I've never been underground, and I'd need to get signoff, and I'm really really bad at securing grant money, if that's what you're after, but...")
exit('PAL')
enter('LEADER', 'b')
speak('LEADER', "Talk with everyone. See what we're like here. We can ease you into it, no issue. Amador's vouched for you so I'm sure you'll be fine, and I'm sure you'll decide we're worth your time.")
speak('YOU', "Okay. Thank you. Really, thank you. I'll talk with everyone. I'll do that.")
exitNVL()

setSwitch('d1_01', true)