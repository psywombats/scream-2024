fade('normal')

wait(1)
enterNVL()
enter('YOU', 'c')
speak('YOU', "AH!")
exit('YOU')
enter('YOU', 'b')
speak('YOU', "Ha.. ha...")
cutin('you')
speak('YOU', "Just the same stupid dream again...")
exit('YOU')
enter('YOU', 'c')
speak('YOU', "Come on, Phoebe, pull it together.")
speak('YOU', "How is it already 10 o'clock? I hope Amador's not too mad I'm late. I'm sorry Amador!")
exitNVL()

setSwitch('d0_00', true)

setString('time', "11:00 AM");
teleport('MeetingMap', 'start')
