enterNVL()
if not getSwitch('d1_01_pal') then
	enter('YOU', 'd')
	speak('YOU', "I didn't know you were into caving, Amador. When we were kids, you only did track.")
	enter('PAL', 'b')
	speak('PAL', "Track, and then rowing and skiing and bouldering. And backcountry hiking when I was in undergrad. That was a blast.")
	speak('YOU', "Sounds like you. Every outdoor activity under the sun.")
	speak('PAL', "So I figured I'd branch out! Try something under the earth instead. Turns out caving is extremely my thing.")
	speak('YOU', "Well, I'm glad you've found someplace you belong. I'm really nervous. I hope I belong here too. This is basically my last shot at my doctorate.")
end
speak('PAL', "Just take a breath and introduce yourself to everyone. You'll be fine.")
exitNVL()

setSwitch('d1_01_pal', true)
play('d1_01_next')
