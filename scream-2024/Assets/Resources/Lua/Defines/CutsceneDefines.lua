-- global defines for cutscenes

function teleportCoords(mapName, x, y)
    cs_teleportCoords(mapName, x, y)
    await()
end

function teleport(mapName, eventName, dir, raw)
    cs_teleport(mapName, eventName, dir, raw)
    await()
end

function targetTele(mapName, eventName, dir, raw)
    cs_teleport(mapName, eventName, dir, raw)
    await()
end

function fadeOutBGM(seconds)
    cs_fadeOutBGM(seconds)
    await()
end

function speak(speaker, line)
    cs_speak(speaker, line)
    await()
end

function radio(speaker, line, qual)
	cs_radio(speaker, line, qual)
	await()
end

function fade(fadeType, dur)
    cs_fade(fadeType, dur)
    await()
end

function enterNVL(arg)
    cs_enterNVL(arg)
    await()
end

function exitNVL()
    cs_exitNVL()
    await()
end

function rotateTo(event)
	cs_rotateTo(event)
	await()
end

function lookAt(event)
	cs_rotateTo(event)
	await()
end


function bootGazer(on)
	cs_bootGazer(on)
	await()
end

function enter(speaker, slot, alt)
    cs_enter(speaker, slot, alt)
    await()
end

function exit(speaker)
    cs_exit(speaker)
    await()
end

function expr(speaker, expression)
	cs_expr(speaker, expression)
	await()
end

function clue(item)
	cs_clue(item)
	await()
	return selection
end

function cutin(key)
	cs_cutin(key)
	await()
end

function intertitle(s1, s2)
	cs_intertitle(s1, s2)
	await()
end

function walk(event, count, direction, wait)
    if wait == nil then wait = true end
    cs_walk(event, count, direction, wait)
    if wait then
        await()
    end
end

