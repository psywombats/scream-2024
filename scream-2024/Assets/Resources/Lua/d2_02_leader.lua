enterNVL()
enter('YOU', 'e')
enter('LEADER', 'c')
if not getSwitch('d2_02_leader') then
	speak('LEADER', "Look at you, Phoebe. Second day on the job and you're already pulling your own weight.")
	speak('YOU', "My own weight and more. What's all the equipment for?")
	speak('LEADER', "Some of it is Nika's stuff, some of it's ropes and things we need to keep exploring, and some of it's rations and sleeping bags for if we decide to overnight down here.")
	speak('LEADER', "We waste so much time going over territory we've already mapped. This should increase our daily efficiency by a huge factor.")
	speak('YOU', "Is there a reason you all are working so fast?")
	speak('LEADER', "Nika needs Spelonky done by the end of the semester, but, well, more than that...")
	speak('LEADER', "We can't maintain secrecy forever. This is the golden hour for us down here, the best opportunity we'll ever get.")
	speak('LEADER', "Don't you feel something calling to you from the bottom of this cave?")
	speak('YOU', "Sort of, but...")
	speak('LEADER', "Like I said before, unexplored places are sacred. I feel like it's my destiny to reach the bottom of this cave, first, and experience the secret.")
	speak('YOU', "I don't understand. What's at the bottom? Something special?")
	exit('LEADER')
	enter('LEADER', 'b')
	speak('LEADER', "No one knows the answer to that question. That, precisely, is what makes it special.")
	speak('LEADER', "I just... need to do this. I won't be complete if I don't set eyes down there. I'd honestly rather die in here than surface and know I'd never come back.")
	speak('YOU', "There are some times I feel like that with my doctorate... If I can't achieve my goals, what's the point? Why be alive? Why keep going?")
end
speak('LEADER', "I knew I could count on you. You're just as determined as me. And that's why I know you won't betray us.")
exitNVL()

setSwitch('d2_02_leader', true)
play('d2_02_next')