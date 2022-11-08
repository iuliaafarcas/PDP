import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

public class main {
    public static ArrayList<ArrayList<Long>> createMatrix(int n){
        ArrayList<ArrayList<Long>> list_= new ArrayList<>();
        for(int i=0;i<n;i++)
        {
            list_.add(new ArrayList<>());
            for(int j=0;j<n;j++){
                Random rand = new Random();
                list_.get(i).add(j, rand.nextLong()%10);
            }
        }
        return list_;
    }

    public static ArrayList<ArrayList<Long>> createEmptyMatrix(int n){
        ArrayList<ArrayList<Long>> list_= new ArrayList<>();
        for(int i=0;i<n;i++)
        {
            list_.add(new ArrayList<>());
            for(int j=0;j<n;j++){
                list_.get(i).add(j, 0L);
            }
        }
        return list_;
    }
    public static void main(String[] args) throws InterruptedException {
        int n =50;
        Matrix leftMatrix = new Matrix(n, createMatrix(n));
        Matrix rightMatrix = new Matrix (n, createMatrix(n));
        Matrix resultMatrix = new Matrix(n, createEmptyMatrix(n));

        Long startTime = System.nanoTime();
        Long threadCount = 8L;
        // with threads
        List<Thread> threadList = new ArrayList<>();
        for (Long i=0L; i <threadCount;i++){
 //           Thread computingThread = new Thread(new Thread1(leftMatrix,rightMatrix,resultMatrix,i,threadCount));
 //           Thread computingThread = new Thread(new Thread1(leftMatrix,rightMatrix,resultMatrix,i,threadCount));
            Thread computingThread = new Thread(new Thread1(leftMatrix,rightMatrix,resultMatrix,i,threadCount));
                threadList.add(computingThread);
                computingThread.start();
        }
        for(Thread thread: threadList){
            thread.join();
        }

//        ExecutorService executorService= Executors.newFixedThreadPool(4);
//        for (Long i=0L; i< threadCount;i++){
//            Runnable worker = new Thread1(leftMatrix,rightMatrix,resultMatrix,i,threadCount);
//// Runnable worker = new Thread2(leftMatrix,rightMatrix,resultMatrix,i,threadCount);
////           Runnable worker = new Thread3(leftMatrix,rightMatrix,resultMatrix,i,threadCount);
//            executorService.execute(worker);
//        }
//
//        executorService.shutdown();
//        executorService.awaitTermination(1000, TimeUnit.HOURS);
        Long stopTime = System.nanoTime();
        long timeTaken=(stopTime-startTime)/1000000;
        System.out.println("---");
        System.out.println("FINISHED");
        System.out.println("Time taken: "+ timeTaken);

    }
}
