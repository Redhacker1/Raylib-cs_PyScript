import clr
clr.AddReference('raylib_cs')
from Raylib_cs import *
import time

Raylib.DrawText("ERROR!", 0,100, 60, Color.RED)

def Main():
	string_1 = "800"
	String_2 = "0000000000000000"
	string_item = string_1 + String_2

	Raylib.DrawText(string_item, 0,0, 60, Color.GREEN)



