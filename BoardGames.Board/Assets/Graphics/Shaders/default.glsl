#type vertex
#version 330 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aUV;
out vec4 vertexColor;
out vec2 uv;

uniform mat4 projection;
uniform mat4 model;

void main() {
    vertexColor = aColor;
    uv = aUV;
    gl_Position = projection * model * vec4(aPosition.xy, 0, 1.0);
}

#type fragment
#version 330 core
out vec4 FragColor;
in vec4 vertexColor;
in vec2 uv;

uniform sampler2D texture0;

void main() {
    FragColor = texture(texture0, uv) * vertexColor;
}
