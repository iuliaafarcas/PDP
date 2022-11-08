public class Thread2 implements Runnable {
    public final Matrix leftMatrix;
    public final Matrix rightMatrix;
    public Matrix finalMatrix;
    public Long totalThreadNumber;
    public Long threadNumber;

    public Thread2(Matrix leftMatrix, Matrix rightMatrix, Matrix finalMatrix,Long threadNumber, Long totalThreadNumber) {
        this.leftMatrix = leftMatrix;
        this.rightMatrix = rightMatrix;
        this.finalMatrix = finalMatrix;
        this.totalThreadNumber = totalThreadNumber;
        this.threadNumber = threadNumber;
    }

    @Override
    public void run() {
        int resultSize = this.leftMatrix.getTotalNumberOfElements();
        Long elementCount = Long.valueOf(resultSize / totalThreadNumber);
        Long startingEl = threadNumber * elementCount;
        Long endingEl;
        if (this.threadNumber == this.totalThreadNumber - 1) {
            endingEl = this.totalThreadNumber;
        } else {
            endingEl = (this.threadNumber + 1) * elementCount;
        }
        for (Long i = startingEl; i < endingEl; i++) {
            int y = (int) (i / this.leftMatrix.getSize());
            int x = (int) (i % this.leftMatrix.getSize());
            this.finalMatrix.setElement( x, y,Matrix.computeMultiplicationElement(this.leftMatrix, this.rightMatrix,x,y));
        }
    }
}
