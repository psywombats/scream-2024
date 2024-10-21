enterNVL()
enter('YOU', 'b')
enter('JERK', 'c')
if not getSwitch('d2_02_jerk') then
	speak('JERK', "You're nervous.")
	speak('YOU', "I'm not nervous. I'm just - ")
	speak('JERK', "You won't stop pacing around and you won't make eye contact.")
	speak('YOU', "I'm not making eye contact because your headlamp is blinding me.")
	speak('JERK', "I apologize.")
	exit('JERK')
	enter('JERK', 'd')
	speak('JERK', "I wasn't trying to scare you earlier, and I'm not trying to accuse you of anything now. I am just... not quite sure why Jeanne would put her whole expedition in danger to take along a novice like you.")
	speak('YOU', "You claim you're not trying to accuse me of anything, but, you're being pretty blunt.")
	speak('YOU', "And, I mean, I guess I'm not an expert caver like you, but, I'm down here with you now, right? I made it this far.")
	speak('JERK', "True enough.")
	speak('JERK', "I'm just... bothered by this setup.")
	speak('JERK', "I'm a scientist, like you. My field is geology. And this cave doesn't quite line up with what I know about geology.")
	speak('JERK', "I don't understand what the original miners were doing in the upper levels. Stopes usually follow natural ore deposits, but the mines up there were...")
	speak('JERK', "And the natural caves down here, limestone shouldn't...")
	exit('JERK')
	enter('JERK', 'c')
	speak('JERK', "My point is that this all mystery. It's fascinating to me. It should be fascinating to anyone, for obvious reasons, and I understand your enthusiasm.")
	speak('JERK', "But your presence here makes us all less safe. You, especially, are in danger. The more you appreciate that, the safer you'll be.")
	speak('YOU', "Thanks. I think.")
	speak('JERK', "It wasn't anything to thank me about.")
	speak('JERK', "Now let me get back to this formation here...")
	exit('JERK')
	enter('JERK', 'e')
end
speak('JERK', "...just don't get it. This should be the chance of a lifetime, so why does it feel so ...wrong... down here?")
exitNVL()

setSwitch('d2_02_jerk', true)
play('d2_02_next')