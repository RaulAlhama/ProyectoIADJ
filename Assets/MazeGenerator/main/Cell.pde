class Cell {
  int i, j, w, h;
  boolean[] walls = {true, true, true, true};
  boolean visited = false;

  Cell(int i, int j, int w) {
    this.i = i;
    this.j = j;
    this.w = w;
    this.h = i+j;
  }

  String toString() {
    return "i="+i+", j="+j+", visitado="+visited+" # "  ;
  }

  void highlight() {
    int x = this.i*this.w;
    int y = this.j*this.w;
    noStroke();
    fill(0, 0, 255, 100);
    rect(x, y, this.w, this.w);
  }


  ArrayList getNeighbors(Maze maze) {
    ArrayList<Cell> neighbors = new ArrayList<Cell>();
    
    int index = 0; //<>//
    
    
    // SI SE CAMBIA EL ORDEN DE RECORRIDO, EL CAMINO RESULTANTE PUEDE SER OTRO.
    index = maze.index(i, j-1);
    Cell top = index >= 0 ? maze.grid.get(index): null;
    index = maze.index(i+1, j);
    Cell right = index >0 ? maze.grid.get(index): null;
    index = maze.index(i, j+1);
    Cell bottom = index >=0 ? maze.grid.get(index): null;
    index = maze.index(i-1, j);
    Cell left = index >= 0 ? maze.grid.get(index): null;

    if (top != null && !walls[0]) {
      neighbors.add(top);
    }
    if (right != null && !walls[1]) {
      neighbors.add(right);
    }
    if (bottom != null && !walls[2]) {
      neighbors.add(bottom);
    }
    if (left != null && !walls[3]) {
      neighbors.add(left);
    }
    
    
    return neighbors;
  }
  
  
  
  Cell checkNeighbors(Maze maze) {
    ArrayList<Cell> neighbors = new ArrayList<Cell>();
    
    int index = 0;
    
    index = maze.index(i, j-1);
    Cell top = index >= 0 ? maze.grid.get(index): null;
    index = maze.index(i+1, j);
    Cell right = index >0 ? maze.grid.get(index): null;
    index = maze.index(i, j+1);
    Cell bottom = index >=0 ? maze.grid.get(index): null;
    index = maze.index(i-1, j);
    Cell left = index >= 0 ? maze.grid.get(index): null;


    if (top != null && !top.visited) {
      neighbors.add(top);
    }
    if (right != null && !right.visited) {
      neighbors.add(right);
    }
    if (bottom != null && !bottom.visited) {
      neighbors.add(bottom);
    }
    if (left != null && !left.visited) {
      neighbors.add(left);
    }
    
    
  
    if (neighbors.size() > 0) {
      int r = floor(random(0, neighbors.size()));
      return neighbors.get(r);
    } else {
      return null;
    }
  }


  void show() {
    int x = this.i*this.w;
    int y = this.j*this.w;

    noStroke();
    fill(0, 0, 0);
    rect(x, y, this.w, this.w);

    strokeWeight(6);
    stroke(255);
    if (this.walls[0]) {
      line(x, y, x + this.w, y);
    }
    if (this.walls[1]) {
      line(x + this.w, y, x + this.w, y + w);
    }
    if (this.walls[2]) {
      line(x + this.w, y + this.w, x, y + w);
    }
    if (this.walls[3]) {
      line(x, y + this.w, x, y);
    }


    textSize(this.w/1.5);
    fill(0, 408, 612);
    text(this.h, this.i*this.w+this.w/4, this.j*this.w+this.w/1.5);
  }
}
