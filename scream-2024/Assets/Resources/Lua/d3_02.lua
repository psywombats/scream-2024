enterNVL()
enter('YOU', 'c')
speak('YOU', "Where'd he go?")
expr('YOU', 'dis')
speak('YOU', "This... This is the same spot as yesterday, isn't it?")
exitNVL()

wait(.6)

radio('LEADER', "Is there a problem?")
radio('YOU', "No! No problem at all! Just, hold on a moment over there.")
exitRadio()

setSwitch('d3_02', true)
