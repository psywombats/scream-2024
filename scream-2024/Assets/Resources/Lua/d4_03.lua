radio('LEADER', "Come in, CONTROL.")
radio('CONTROL', "Oh good, you can still - ")
radio('CONTROL', " - thought you'd be out range, but I can still sort of hear - ")
exitRadio()

enterNVL()
enter('YOU', 'c')
enter('JERK', 'b')
enter('LEADER', 'd', 'serious')
speak('JERK', "We're at the max range of even the repeater.")
expr('JERK', 'meh')
speak('JERK', "Whatever these walls are made of, it's not limestone, or any natural stone that I know of. This is powerful equipment. It should be able to penetrate to the surface.")
speak('LEADER', "Then we set up camp here. We leave the repeater here, rest a bit, then search for Amador in earnest.")
expr('JERK', '')
speak('JERK', "I'll work on the repeater. The battery should last at least a day. I'll let you know when I'm done.")
exitNVL()

setSwitch('d4_03', true)
