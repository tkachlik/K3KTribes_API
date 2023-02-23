# Instructions

## General rules

This game is a basic event based Travian like game, where u control and manage
resources and army in a kingdom and try to destroy other players or take control
over their kingdoms tu rule the whole World.

After the registration you start with one kingdom in inner circle on the map
(you need to pick a spot). You start with one farm and one mine to generate some
resources at the begining. From this point forward it's completely on you which
direction this kingdom will go.

NPC kingdoms NOT within current scope of app.

## Map

Map in this game is 20 x 20 squares.Each square represents a spot for a kingdom.
Inner circle is 12x12 squares big and its in the middle. Only in this circle can
player place a new kingdom. Outer circle is meant for NPC players to have their
kingdoms, which u can take control of or destroy.

This will be a 2D array of coordinates (probably in World).

## Kingdom

# Creation
Player can have more kingdoms under his control.They are independent on each
other. Every kingdom has it's own storage, production and army. In the beginning
of the game, a players founds a new kingdom, then they can only take control of 
existing kingdom, not create a new one, unless they have no kingdom left.
(On kingdom creation, check that player's kingdom list is empty, otherwise no creation.)
(May be changed in future update).
On creation, each kingdom has a Townhall (lvl 1) and Farm, also 100 gold.
Research lvl (new property) is set to 1 at game start.
Attack level == all unit attack points
Defense level == all unit defense points + building defense points (i.e. townhall + wall) + % def boost (from wall).
Loyalty level == 100. Losing battle against an army with a senator decreases loyalty by 25. Winning a battle with senator on your side increases loyalty by 25.
(increasing loyalty with celebrations for food and gold possible in future update)

# Production and unit logic
In the case that your food production is lower than 0 and u
dont have enough food in the storage, first your troops start to leave your
kingdom until u have a positive production (+5). If u don't have any more troops
and still have negative food pruduction the food needed will be taken from your
gold production until u build enough farms.

# Max kingdom cap
Any player can have maximum of 3 kingdoms. (See Senator below for implementation detail.)

## Buildings

Every kingdom has it's own buildings.
Maximum buildings in each kingdom by type:

- Farm - 5x
- Mine - 3x
- Others - 1x

There is a requirement that needs to be met for every building. Maximum level of
a building can not exceed level of a townhall in that kingdom. Every building
can be added to the building queue but only one level of that building can be
build at a time. Therefore, different types of building can be built and upgraded parallel,
but upgrade of the same building from e.g. lvl 3 to lvl 4 must happen consequently.
Every building is consuming some amount of food and upgrading cost some gold.

Production time (prodTime) is calculated as timeStep * (buildingLevel - 1) - NOT IMPLEMENTED

We will seed db of bulding and unit types from file with hardcoded data.

Consumption is always in FOOD units and consumption == building level. Therefore, when creating a production, check if kingdom has at least as much FOOD units as is the level of the building that should perform the production. Production result == (production - consumption). (Consider changing the production column in the future csv file to actual production amounts and dropping the consumption column altogether.)

## Townhall

- every level has some basic defense
- other buildings can not exceed level of this building
- maximum storage of a kingdom is based on level of this building
- storage is storage for gold and for food (so lvl1 has 1200 gold and 1200 food capacity)
- timeStep = 15 minutes

| level | cost | prodTime | maxStorage | defense | consumption |
| ----- | ---- | --------- | ---------- | ------- | ----------- |
| 1     | 120  | 1:00:00   | 1200       | 10      | 1           |
| 2     | 135  | 1:15:00   | 2400       | 12      | 2           |
| 3     | 271  | 1:30:00   | 3600       | 15      | 3           |
| 4     | 406  | 1:45:00   | 4800       | 17      | 4           |
| 5     | 542  | 2:00:00   | 6000       | 20      | 5           |
| 6     | 677  | 2:15:00   | 7200       | 22      | 6           |
| 7     | 813  | 2:30:00   | 8400       | 25      | 7           |
| 8     | 949  | 2:45:00   | 9600       | 27      | 8           |
| 9     | 1084 | 3:00:00   | 10800      | 30      | 9           |
| 10    | 1220 | 3:15:00   | 12000      | 32      | 10          |
| 11    | 1355 | 3:30:00   | 13200      | 35      | 11          |
| 12    | 1491 | 3:45:00   | 14400      | 37      | 12          |
| 13    | 1627 | 4:00:00   | 15600      | 40      | 13          |
| 14    | 1762 | 4:15:00   | 16800      | 42      | 14          |
| 15    | 1898 | 4:30:00   | 18000      | 45      | 15          |
| 16    | 2033 | 4:45:00   | 19200      | 47      | 16          |
| 17    | 2169 | 5:00:00   | 20400      | 50      | 17          |
| 18    | 2305 | 5:15:00   | 21600      | 52      | 18          |
| 19    | 2440 | 5:30:00   | 22800      | 55      | 19          |
| 20    | 2576 | 5:45:00   | 24000      | 57      | 20          |

## Farm

- produces food for this kingdom
- timeStep = 2 mins

| level | cost | prodTime | production | consumption |
| ----- | ---- | --------- | ---------- | ----------- |
| 1     | 60   | 0:08:00   | 6          | 1           |
| 2     | 67   | 0:10:00   | 13         | 2           |
| 3     | 135  | 0:12:00   | 20         | 3           |
| 4     | 203  | 0:14:00   | 27         | 4           |
| 5     | 271  | 0:16:00   | 34         | 5           |
| 6     | 338  | 0:18:00   | 41         | 6           |
| 7     | 406  | 0:20:00   | 48         | 7           |
| 8     | 474  | 0:22:00   | 55         | 8           |
| 9     | 542  | 0:24:00   | 62         | 9           |
| 10    | 610  | 0:26:00   | 69         | 10          |
| 11    | 677  | 0:28:00   | 75         | 11          |
| 12    | 745  | 0:30:00   | 82         | 12          |
| 13    | 813  | 0:32:00   | 89         | 13          |
| 14    | 881  | 0:34:00   | 96         | 14          |
| 15    | 949  | 0:36:00   | 103        | 15          |
| 16    | 1016 | 0:38:00   | 110        | 16          |
| 17    | 1084 | 0:40:00   | 117        | 17          |
| 18    | 1152 | 0:42:00   | 124        | 18          |
| 19    | 1220 | 0:44:00   | 131        | 19          |
| 20    | 1288 | 0:46:00   | 138        | 20          |

## Mine

- produces gold for this kingdom
- timeStep = 2,5 minutes

| level | cost | prodTime | production | consumption |
| ----- | ---- | --------- | ---------- | ----------- |
| 1     | 80   | 0:10:00   | 5          | 1           |
| 2     | 90   | 0:12:30   | 11         | 2           |
| 3     | 180  | 0:15:00   | 17         | 3           |
| 4     | 271  | 0:17:30   | 23         | 4           |
| 5     | 361  | 0:20:00   | 28         | 5           |
| 6     | 451  | 0:22:30   | 34         | 6           |
| 7     | 542  | 0:25:00   | 40         | 7           |
| 8     | 632  | 0:27:30   | 46         | 8           |
| 9     | 723  | 0:30:00   | 51         | 9           |
| 10    | 813  | 0:32:30   | 57         | 10          |
| 11    | 903  | 0:35:00   | 63         | 11          |
| 12    | 994  | 0:37:30   | 69         | 12          |
| 13    | 1084 | 0:40:00   | 74         | 13          |
| 14    | 1175 | 0:42:30   | 80         | 14          |
| 15    | 1265 | 0:45:00   | 86         | 15          |
| 16    | 1355 | 0:47:30   | 92         | 16          |
| 17    | 1446 | 0:50:00   | 97         | 17          |
| 18    | 1536 | 0:52:30   | 103        | 18          |
| 19    | 1627 | 0:55:0    | 109        | 19          |
| 20    | 1717 | 0:57:30   | 114        | 20          |

## Barracks

- here you can build new soldiers for your army
- higher level of barracks will shorten the time u need to construct a troop
- timeStep of building = 12,5 minutes
- timeStep of units, see below (decreases)

| level | cost | prodTime  | consumption | axemen  | phalanx | knights | catapult | the spy | senator |
|       |      |           |             |10s      |10s      |20s      |30s       |15s      |45s      | 
| ----- | ---- | --------- | ----------- | ------- | ------- | ------- | -------- | ------- | ------- |
| 1     | 130  | 0:50:00   | 1           | 0:09:50 | 0:09:50 | 0:19:40 | 0:29:30  | 0:14:45 | 0:59:15 |
| 2     | 146  | 1:02:30   | 2           | 0:09:40 | 0:09:40 | 0:19:20 | 0:29:00  | 0:14:30 | 0:58:30 |
| 3     | 293  | 1:15:00   | 3           | 0:09:30 | 0:09:30 | 0:19:00 | 0:28:30  | 0:14:15 | 0:57:45 |
| 4     | 440  | 1:27:30   | 4           | 0:09:20 | 0:09:20 | 0:18:40 | 0:28:00  | 0:14:00 | 0:57:00 |
| 5     | 587  | 1:40:00   | 5           | 0:09:10 | 0:09:10 | 0:18:20 | 0:27:30  | 0:13:45 | 0:56:15 |
| 6     | 734  | 1:52:30   | 6           | 0:09:00 | 0:09:00 | 0:18:00 | 0:27:00  | 0:13:30 | 0:55:30 |
| 7     | 881  | 2:05:00   | 7           | 0:08:50 | 0:08:50 | 0:17:40 | 0:26:30  | 0:13:15 | 0:54:45 |
| 8     | 1028 | 2:17:30   | 8           | 0:08:40 | 0:08:40 | 0:17:20 | 0:26:00  | 0:13:00 | 0:54:00 |
| 9     | 1175 | 2:30:00   | 9           | 0:08:30 | 0:08:30 | 0:17:00 | 0:25:30  | 0:12:45 | 0:53:15 |
| 10    | 1322 | 2:42:30   | 10          | 0:08:20 | 0:08:20 | 0:16:40 | 0:25:00  | 0:12:30 | 0:52:30 |
| 11    | 1468 | 2:55:00   | 11          | 0:08:10 | 0:08:10 | 0:16:20 | 0:24:30  | 0:12:15 | 0:51:45 |
| 12    | 1615 | 3:07:30   | 12          | 0:08:00 | 0:08:00 | 0:16:00 | 0:24:00  | 0:12:00 | 0:51:00 |
| 13    | 1762 | 3:20:00   | 13          | 0:07:50 | 0:07:50 | 0:15:40 | 0:23:30  | 0:11:45 | 0:50:15 |
| 14    | 1909 | 3:32:30   | 14          | 0:07:40 | 0:07:40 | 0:15:20 | 0:23:00  | 0:11:30 | 0:49:30 |
| 15    | 2056 | 3:45:00   | 15          | 0:07:30 | 0:07:30 | 0:15:00 | 0:22:30  | 0:11:15 | 0:48:45 |
| 16    | 2203 | 3:57:30   | 16          | 0:07:20 | 0:07:20 | 0:14:40 | 0:22:00  | 0:11:00 | 0:48:00 |
| 17    | 2350 | 4:10:00   | 17          | 0:07:10 | 0:07:10 | 0:14:20 | 0:21:30  | 0:10:45 | 0:47:15 |
| 18    | 2497 | 4:22:30   | 18          | 0:07:00 | 0:07:00 | 0:14:00 | 0:21:00  | 0:10:30 | 0:46:30 |
| 19    | 2644 | 4:35:00   | 19          | 0:06:50 | 0:06:50 | 0:13:40 | 0:20:30  | 0:10:15 | 0:45:45 |
| 20    | 2791 | 4:47:30   | 20          | 0:06:40 | 0:06:40 | 0:13:20 | 0:20:00  | 0:10:00 | 0:45:00 |

## Academy

- here you can increse level (attack and defense power) of your soldiers in this kingdom
- increasing Academy lvl itself is not enough, player must also perform research to increase research level
- research level is a property in the KINGDOM model
- conducting research increases the lvl of NEWLY produced units (already existing units keep their level)
- max possible research level == Academy building level +1
- level of troops == research level
- upgrade all troop types at once but only one level for each type at a time (same as building queue logic)
- timeStep of building = 17,5 minutes
- timeStep of research = 8 minutes

| level | cost | prodTime  | researchTime | consumption |
| ----- | ---- | --------- | ------------ | ----------- |
| 1     | 140  | 1:10:00   | 0:48:0       | 1           |
| 2     | 158  | 1:27:30   | 0:56:0       | 2           |
| 3     | 316  | 1:45:00   | 1:04:0       | 3           |
| 4     | 474  | 2:2:30    | 1:12:0       | 4           |
| 5     | 632  | 2:20:00   | 1:20:0       | 5           |
| 6     | 790  | 2:37:30   | 1:28:0       | 6           |
| 7     | 949  | 2:55:00   | 1:36:0       | 7           |
| 8     | 1107 | 3:12:30   | 1:44:0       | 8           |
| 9     | 1265 | 3:30:00   | 1:52:0       | 9           |
| 10    | 1423 | 3:47:30   | 2:0:0        | 10          |
| 11    | 1581 | 4:5:00    | 2:8:0        | 11          |
| 12    | 1740 | 4:22:30   | 2:16:0       | 12          |
| 13    | 1898 | 4:40:00   | 2:24:0       | 13          |
| 14    | 2056 | 4:57:30   | 2:32:0       | 14          |
| 15    | 2214 | 5:15:00   | 2:40:0       | 15          |
| 16    | 2373 | 5:32:30   | 2:48:0       | 16          |
| 17    | 2531 | 5:50:00   | 2:56:0       | 17          |
| 18    | 2689 | 6:7:30    | 3:4:0        | 18          |
| 19    | 2847 | 6:25:00   | 3:12:0       | 19          |

## Wall

- each level has some base defense which helps with defending agains attacks
- each level increases defense power of your army by 2 %
- timeStep = 7,5 minutes

| level | cost | prodTime | defense | def boost | consumption |
| ----- | ---- | --------- | ------- | --------- | ----------- |
| 1     | 100  | 0:30:00   | 20      | 2 %       | 1           |
| 2     | 112  | 0:37:30   | 25      | 4 %       | 2           |
| 3     | 225  | 0:45:00   | 30      | 6 %       | 3           |
| 4     | 338  | 0:52:30   | 35      | 8 %       | 4           |
| 5     | 451  | 1:0:00    | 40      | 10 %      | 5           |
| 6     | 565  | 1:7:30    | 45      | 12 %      | 6           |
| 7     | 677  | 1:15:00   | 50      | 14 %      | 7           |
| 8     | 790  | 1:22:30   | 55      | 16 %      | 8           |
| 9     | 903  | 1:30:00   | 60      | 18 %      | 9           |
| 10    | 1016 | 1:37:30   | 65      | 20 %      | 10          |
| 11    | 1130 | 1:45:00   | 70      | 22 %      | 11          |
| 12    | 1242 | 1:52:30   | 75      | 24 %      | 12          |
| 13    | 1355 | 2:0:00    | 80      | 26 %      | 13          |
| 14    | 1468 | 2:7:30    | 85      | 28 %      | 14          |
| 15    | 1581 | 2:15:00   | 90      | 30 %      | 15          |
| 16    | 1694 | 2:22:30   | 95      | 32 %      | 16          |
| 17    | 1807 | 2:30:00   | 100     | 34 %      | 17          |
| 18    | 1920 | 2:37:30   | 105     | 36 %      | 18          |
| 19    | 2033 | 2:45:00   | 110     | 38 %      | 19          |
| 20    | 2147 | 2:52:30   | 115     | 40 %      | 20          |

## Troops

Your army consists of individual troops u create based on type. troop base stats
same logic as Building

| unit type | cost  | attack | defense | carryCap | consumption | speed |
| --------- | ----- | ------ | ------- | -------- | ----------- | ----- |
| Axemen    | 20    | 8.0    | 5.0     | 30       | 1           | 1     |
| Pahalanx  | 20    | 5.0    | 8.0     | 30       | 1           | 0.8   |
| Knight    | 50    | 15.0   | 10.0    | 50       | 2           | 1.6   |
| Catapult  | 300   | 0      | 0       | 0        | 3           | 0.5   |
| Spy       | 60    | 4.0    | 3.0     | 0        | 2           | 2.5   |
| Senator   | 5000  | 0      | 0       | 0        | 5           | 0.5   |

## Axemen

- have more attacking power not so effective on defense

| level | cost | attack | defense |
| ----- | ---- | ------ | ------- |
| 1     | 77   | 8.4    | 5.3     |
| 2     | 154  | 8.8    | 5.5     |
| 3     | 231  | 9.2    | 5.8     |
| 4     | 308  | 9.6    | 6.0     |
| 5     | 385  | 10.0   | 6.3     |
| 6     | 462  | 10.4   | 6.5     |
| 7     | 539  | 10.8   | 6.8     |
| 8     | 616  | 11.2   | 7.0     |
| 9     | 693  | 11.6   | 7.3     |
| 10    | 770  | 12.0   | 7.5     |
| 11    | 847  | 12.4   | 7.8     |
| 12    | 924  | 12.8   | 8.0     |
| 13    | 1001 | 13.2   | 8.3     |
| 14    | 1078 | 13.6   | 8.5     |
| 15    | 1155 | 14.0   | 8.8     |
| 16    | 1232 | 14.4   | 9.0     |
| 17    | 1309 | 14.8   | 9.3     |
| 18    | 1386 | 15.2   | 9.5     |
| 19    | 1463 | 15.6   | 9.8     |
| 20    | 1540 | 16.0   | 10.0    |

## Phalanx

- better defensive attributes but still somewhat effective in attack with the
  same carrying capacity

| level | cost | attack | defense |
| ----- | ---- | ------ | ------- |
| 1     | 77   | 5.3    | 8.4     |
| 2     | 154  | 5.5    | 8.8     |
| 3     | 231  | 5.8    | 9.2     |
| 4     | 308  | 6.0    | 9.6     |
| 5     | 385  | 6.3    | 10.0    |
| 6     | 462  | 6.5    | 10.4    |
| 7     | 539  | 6.8    | 10.8    |
| 8     | 616  | 7.0    | 11.2    |
| 9     | 693  | 7.3    | 11.6    |
| 10    | 770  | 7.5    | 12.0    |
| 11    | 847  | 7.8    | 12.4    |
| 12    | 924  | 8.0    | 12.8    |
| 13    | 1001 | 8.3    | 13.2    |
| 14    | 1078 | 8.5    | 13.6    |
| 15    | 1155 | 8.8    | 14.0    |
| 16    | 1232 | 9.0    | 14.4    |
| 17    | 1309 | 9.3    | 14.8    |
| 18    | 1386 | 9.5    | 15.2    |
| 19    | 1463 | 9.8    | 15.6    |
| 20    | 1540 | 10.0   | 16.0    |

## Knight

- more expensive unit with excelent speed and carrying capacity for quick
  robberies

| level | cost | attack | defense |
| ----- | ---- | ------ | ------- |
| 1     | 121  | 15.8   | 10.5    |
| 2     | 242  | 16.5   | 11.0    |
| 3     | 363  | 17.3   | 11.5    |
| 4     | 484  | 18.0   | 12.0    |
| 5     | 605  | 18.8   | 12.5    |
| 6     | 726  | 19.5   | 13.0    |
| 7     | 847  | 20.3   | 13.5    |
| 8     | 968  | 21.0   | 14.0    |
| 9     | 1089 | 21.8   | 14.5    |
| 10    | 1210 | 22.5   | 15.0    |
| 11    | 1331 | 23.3   | 15.5    |
| 12    | 1452 | 24.0   | 16.0    |
| 13    | 1573 | 24.8   | 16.5    |
| 14    | 1694 | 25.5   | 17.0    |
| 15    | 1815 | 26.3   | 17.5    |
| 16    | 1936 | 27.0   | 18.0    |
| 17    | 2057 | 27.8   | 18.5    |
| 18    | 2178 | 28.5   | 19.0    |
| 19    | 2299 | 29.3   | 19.5    |
| 20    | 2420 | 30.0   | 20.0    |

## Spy

- this unit is fast with very low attack and defense stats
- this unit is here only for one puropose and that is to get info about
  oponent's kingdom

| level | cost | attack | defense |
| ----- | ---- | ------ | ------- |
| 1     | 121  | 4.2    | 3.2     |
| 2     | 242  | 4.4    | 3.3     |
| 3     | 363  | 4.6    | 3.4     |
| 4     | 484  | 4.8    | 3.6     |
| 5     | 605  | 5.0    | 3.8     |
| 6     | 726  | 5.2    | 3.9     |
| 7     | 847  | 5.4    | 4.1     |
| 8     | 968  | 5.6    | 4.2     |
| 9     | 1089 | 5.8    | 4.3     |
| 10    | 1210 | 6.0    | 4.5     |
| 11    | 1331 | 6.2    | 4.7     |
| 12    | 1452 | 6.4    | 4.8     |
| 13    | 1573 | 6.6    | 4.9     |
| 14    | 1694 | 6.8    | 5.1     |
| 15    | 1815 | 7.0    | 5.3     |
| 16    | 1936 | 7.2    | 5.4     |
| 17    | 2057 | 7.4    | 5.6     |
| 18    | 2178 | 7.6    | 5.7     |
| 19    | 2299 | 7.8    | 5.9     |
| 20    | 2420 | 8.0    | 6.0     |

## Senator

- very expensive unit able to take control of another kingdom after a few
  succesfull attacks



  

## BATTLE LOGIC - TBD

There are 4 different types of battle u can engage. What type of a battle u
engage in depends on type of troops u send in that particular battle. Your army is
moving around the map based on the slowest unit you send in that attack.

### PLunder

- this type of battle will be default if u dont send a special unit
- only this type of attack will give u loot from the defending kingdom (based on
  carrying capacity of troops u send)
- units that count to normal plunder attack are: *axemen*, *phalanx*, *knights*

### Intelligence

- this type of attack is trigered only if u send just the spies into the fight
- this type will get information about troops in the kingdom present and
  information about buildings
- only one of your spies needs to survive after battle to be succesfull
- this type of attack you can loose troops only as a attacker, as a defender you
  can only detect and defeat some or all of the attacker spies
- no loot from this battle

### Destruction

- here you need to send some catapults
- every catapult will destroy one level of a random building in defending
  kingdom
- before ms machines can do their job the rest of the army needs to be
  victorious (infantry, cavalry, spies)
- no loot from this battle

### Takeover

- when you send a senator in the battle and your army is victorious, senator
  will decrease loyalty of defending kingdom by 25
- when loyalty comes to 0 attacker takes control over that kingdom and loyalty
  is set back to 100
- no loot from this battle

### Deminishing returns

In every battle there are deminishing returns if a looser's army power is less
than 60% of the attacker army power there are deminishing returns and army is
much less effective.
