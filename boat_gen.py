#this script is pretty much so I can have a multi-language repo

boat_string = ''' WWWWWWWWWWW
WWFFFFFFFFFFWW
WWFFFFFFFFFFFFW
WWFFFFFFFFFFWW
 WWWWWWWWWWW'''

boat_lines = boat_string.split('\n')
for lineno, line in enumerate(boat_lines):
	for charno, boat_char in enumerate(list(line)):
		if boat_char == 'W':
			tile = 12
		elif boat_char == 'F':
			tile = 11
		elif boat_char == 'B':
			tile = 13
		elif boat_char == "C":
			tile = 14
		elif boat_char == " ":
			tile = 0
		else:
			raise ValueError("Bad input char {}".format(boat_char))
		print 'Program.world[Player.pos[0] + {}, Player.pos[1] + {}] = {};'.format(lineno, charno, tile)