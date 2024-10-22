enterNVL()
enter('YOU', 'b')
enter('PAL', 'd')
if not getSwitch('d3_04_pal') then
	speak('YOU', "Hi, Amador. Are you alright?")
	speak('PAL', "Sorry, Phoebe. Just tuckered out. It feels like since we've gotten back, Nika keeps getting mad at her laptop, and Tom and Jeanne have done nothing but fight.")
	speak('PAL', "I don't know what there is to argue about. Let's just come back tomorrow and go back down. Simple as that.")
	speak('YOU', "You're not concerned at all?")
	expr('PAL', 'huh')
	speak('PAL', "Concerned about what? I love caving. It's fantastic. It's thrilling to explore someplace new with my friends. So let's just do that, right?")
	expr('PAL', 'grin')
	speak('PAL', "There's no point in sitting around up here when right now, we could be going back down there, right?")
	expr('PAL', '')
end
speak('PAL', "I swear it's like you and I are the only sane ones left.")
exitNVL()

setSwitch('d3_04_pal', true)
play('d3_04_next')
