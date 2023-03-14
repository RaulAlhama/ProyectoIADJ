
Maze maze;
Agent agente;
int w;

void setup() {
  frameRate(15);  // <<<<<< Cambia el valor para que vaya más rápido o más lento.
  randomSeed(2);
  size(700, 700);
  w=70;
  maze = new Maze(w);
  maze.generate(w);
  agente = new Agent();
}

void draw() {
  background(51);
  maze.draw();
  agente.draw(w);
  if (!keyPressed)  // <<<< Si presionas una tecla no avanzará el agente.
    agente.update(maze);
}
