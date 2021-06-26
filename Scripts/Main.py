import clr
clr.AddReference('raylib_cs')

# Raylib is undefined
from Raylib_cs import *

import time

# RedSkittleFox 
import sys

# Undefined
Raylib.DrawText("ERROR!", 0,100, 60, Color.RED)

# RedSkittleFox: Main missing argv
def Main():
	print("Hello")
	string_1 = "800"
	String_2 = "0000000000000000"
	string_item = string_1 + String_2

    # Undefined
	Raylib.DrawText(string_item, 0,0, 60, Color.GREEN)


