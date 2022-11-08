import java.util.ArrayList;

public class Matrix {
    private final Integer n;

    private ArrayList<ArrayList<Long>> matrix;

    public Matrix(Integer n, ArrayList<ArrayList<Long>> matrix) {
        this.n = n;

        this.matrix = matrix;
    }

    public Long getElement(Integer n, Integer m) {
        return this.matrix.get(n).get(m);
    }

    public void setElement(Integer n, Integer m, Long newValue) {
        this.matrix.get(n).set(m, newValue);
    }

    public Integer getSize() {
        return this.n;
    }

    public Integer getTotalNumberOfElements() {
        return this.n * this.n;
    }

    public static Long computeMultiplicationElement(Matrix leftMatrix, Matrix rightMatrix, int x, int y) {
        Long element = 0L;
        for (int i = 0; i < leftMatrix.getSize(); i++) {
            element += leftMatrix.getElement(x, i) * rightMatrix.getElement(i, y);
        }
        return element;
    }
}
