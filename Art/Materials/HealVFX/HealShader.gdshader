shader_type canvas_item;

void vertex()
{
	// Called for every vertex the material is visible on.
}

void fragment()
{
	// Called for every pixel the material is visible on.
	COLOR = vec4(0.50,1.0,0.40, texture(TEXTURE, UV).a);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
