class Agent {
  int x, y;
  
  Agent() {
    this.x = 1;
    this.y = 0;
  }
  
  void draw(int w) {
    noStroke();
    fill(255,0, 255, 50);
    circle(this.x*w+w/2, this.y*w+w/2, w/2);
  }
  
  void update(Maze m) {
    if (this.x == 0 && this.y == 0) {  // Si estoy en el origen se termina
      noLoop();
      return;
    }
    
    Cell cell = m.grid.get(m.index(this.x, this.y));   // Celda donde está el agente 
    ArrayList<Cell> neighbors = new ArrayList<Cell>();
    neighbors = cell.getNeighbors(maze); // Los vecinos de la celda
      
    // Si hay vecinos
    if (neighbors.size() > 0) {
      
      // Detectar el de mínimo coste
      Cell cell_min = neighbors.get(0);
      int h_min = neighbors.get(0).h;
      
      for (int i=1; i<neighbors.size(); i++) {
        if (neighbors.get(i).h < h_min) {
          cell_min = neighbors.get(i);
          h_min = neighbors.get(i).h;
        }
      }
      
      // Actualizar cost de la celda actual
      cell.h = h_min+1;
      
      // Mover al agente
      this.x = cell_min.i; 
      this.y = cell_min.j;
      
    } else {
      exit();
    }
  }
}
