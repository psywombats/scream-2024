radio('LEADER', "That's as far as we go today.")
radio('YOU', "Sounds fine to me. I'm exhausted.")
radio('JERK', "You've barely scratched the surface.")
radio('CONTROL', "Tom! Don't be mean. You guys are almost at the end of Entry Chasm. It took us almost a month to map that place!")
radio('LEADER', "Let's meet up at the surface, Nika. I've got new scans from Bitsy for you, and we can regroup and plan for tomorrow.")
radio('YOU', "You're coming back down tomorrow? That soon?")
radio('LEADER', "We've only got a limited amount of time before word gets out. And if that happens, your chance to find new species goes with it, so don't get the wrong idea about our pace.")
radio('YOU', "Sorry, I wasn't questioning it. I understand. I want to go even deeper too.")
radio('PAL', "C'mon, everyone. Let's head for the surface and talk there.")

setSwitch('d1_05', true)
setString('time', "9:30 PM");
teleport('MeetingMap', 'start')
