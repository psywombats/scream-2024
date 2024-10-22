enterNVL()
enter('CONTROL', 'b')
enter('YOU', 'd')
if not getSwitch('d3_04_control') then
	speak('CONTROL', "I'm sick of everyone arguing, and I'm sick of sitting around up here tinkering with Spelonky.")
	speak('YOU', "Still dealing with bugs?")
	speak('CONTROL', "I think it's got to be bad data from Bitsy.")
	expr('CONTROL', 'grump')
	speak('CONTROL', "If the scans are accurate, then you guys are pranking me.")
	speak('CONTROL', "Like, the cave that Spelonky spits out is total nonsense. The chambers overlap with each other. It's way too deep. It's non-Euclidean garbage and feeding to to Spelonky generates glitchy nightmare hellscapes.")
	expr('CONTROL', '')
	speak('CONTROL', "The cave really does exist, right?")
	speak('YOU', "Yes?")
	speak('CONTROL', "Okay. I swear, If this is some sort of long-term plan from Amador to troll me, I'd murder him. It's not funny.")
	speak('CONTROL', "If this place is half as mysterious as the data you're bringing back up implies, I want to go down there so badly. Like I literally dream about it.")
	speak('YOU', "That is, uh, relatable.")
	expr('CONTROL', 'yay')
	speak('CONTROL', "I wonder if it's the spirit of that dead guy that's messing with the scanning equipment. Do you think that could happen?")
	speak('YOU', "I'm not exactly an expert in the metaphysical.")
	expr('CONTROL', '')
end
speak('CONTROL', "I guess an actual ghost would have something better to do than haunt a LIDAR system. You're right.")
speak('YOU', "Thanks?")
exitNVL()

setSwitch('d3_04_control', true)
play('d3_04_next')
