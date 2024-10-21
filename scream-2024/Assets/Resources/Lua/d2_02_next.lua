if not getSwitch('d2_02_pal') or not getSwitch('d2_02_leader') or not getSwitch('d2_02_jerk') then
	return
end

wait(1)

enterNVL()
enter('YOU', 'c')
speak('YOU', "I'd better take Jeanne's advice and see if I can find any new branches off from the camp.")
exitNVL()

setSwitch('d2_02_next', true)