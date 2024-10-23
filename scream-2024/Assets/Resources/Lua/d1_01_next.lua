if not getSwitch('d1_01_leader') or not getSwitch('d1_01_control') or not getSwitch('d1_01_jerk') or not getSwitch('d1_01_pal') then
	return
end

showBasics(false)
wait(.7)
teleport('Clubhouse', 'd1_next')
wait(.5)
play('d1_02')