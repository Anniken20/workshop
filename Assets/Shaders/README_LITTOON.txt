LIT TOON MANUAL
------

Public Properties: 

- Toon Ramp Smoothness (TRS)
- Toon Ramp Tinting (TRT)
- Toon Ramp Offset (TRO)
- Texture (Tex)
- Rim Light Power (RLP)
- Brightness (Bght)
- Tiling (Til)
- Rim Light Color (RLC)
- Normal (NM)


TRS - float value that determines the softness / hardness of the cel shading. 

TRT - RGB value that determines the shader hue. 

TRO - float value that determines the offset of the smoothstep node. 

Tex - 2D texture asset that determines the base albedo map (base texture).

RLP - float value that determines the area of the rim light. The lower it goes (>0) the more coverage it will have. 

Bght - float value that determines the intensity of the rim light. 

Til - Texture property. Tiles the texture. 

RLC - RGB value that determines the rim light hue.

Normal - Texture property. Normal Map. 

-----

Every exposed property is editable in the inspector after setting material's shader to Shader Graphs/LIT TOON, or creating new materials from the shader graph asset. To edit the values in the shader graph, navigate to the left top window with all the variables and click on any variable you need to change. They will show up in the Graph Inspector window with all of the related properties. 
