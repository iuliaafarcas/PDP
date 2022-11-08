public class Thread3 implements Runnable {
    public final Matrix leftMatrix;
    public final Matrix rightMatrix;
    public Matrix finalMatrix;
    public Long totalThreadNumber;
    public Long threadNumber;

    public Thread3(Matrix leftMatrix, Matrix rightMatrix, Matrix finalMatrix,Long threadNumber, Long totalThreadNumber) {
        this.leftMatrix = leftMatrix;
        this.rightMatrix = rightMatrix;
        this.finalMatrix = finalMatrix;
        this.totalThreadNumber = totalThreadNumber;
        this.threadNumber = threadNumber;
    }

    @Override
    public void run() {
        int resultSize = this.leftMatrix.getTotalNumberOfElements();
        for (Long i = threadNumber; i < resultSize; i+=totalThreadNumber) {
            int x = (int) (i / this.leftMatrix.getSize());
            int y = (int) (i % this.leftMatrix.getSize());
            this.finalMatrix.setElement( x, y,Matrix.computeMultiplicationElement(this.leftMatrix, this.rightMatrix,x,y));
        }
    }
}
