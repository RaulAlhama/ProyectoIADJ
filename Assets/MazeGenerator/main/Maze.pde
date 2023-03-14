public  class Maze {
  public int cols=0, rows=0;

  ArrayList<Cell> grid;

  public Maze(int w) {
    this.grid = new ArrayList<Cell>();
    init_grid(w);
  }

  void init_grid(int w) {
    this.cols = floor(width/w);
    this.rows = floor(height/w);

    for (int j=0; j<rows; j++)
      for (int i=0; i<cols; i++) {
        Cell cell = new Cell(i, j, w);
        grid.add(cell);
      }
  }

  void generate(int w) {
    ArrayList<Cell> stack = new ArrayList<Cell>();

    Cell current = maze.grid.get(0);
    current.visited = true;
    boolean finish = false;
    
    while (!finish) {
      redraw();
      // current.highlight();

      // STEP 1
      Cell next = current.checkNeighbors(maze);
      if (next != null) {
        next.visited = true;

        // STEP 2
        stack.add(current);
        //for (int i =0; i<stack.size(); i++)
        //  print(stack.get(i));

        //println();

        // STEP 3
        maze.removeWalls(current, next);

        // STEP 4
        current = next;
      } else if (stack.size() > 0) {
        current = stack.remove(stack.size()-1);
        if (stack.size()==0)
          finish = true;
      }
    }
  }


  int index(int i, int j) {
    if (i < 0 || j < 0 || i > this.cols-1 || j > this.rows-1) {
      return -1;
    }
    return i + j * this.cols;
  }

  void removeWalls(Cell a, Cell b) {
    int x = a.i - b.i;
    if (x == 1) {
      a.walls[3] = false;
      b.walls[1] = false;
    } else if (x == -1) {
      a.walls[1] = false;
      b.walls[3] = false;
    }
    int y = a.j - b.j;
    if (y == 1) {
      a.walls[0] = false;
      b.walls[2] = false;
    } else if (y == -1) {
      a.walls[2] = false;
      b.walls[0] = false;
    }
  }


  void draw() {
    for (int i = 0; i < grid.size(); i++) {
      grid.get(i).show();
    }
  }
}
