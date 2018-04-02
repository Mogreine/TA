package Lab1;

import java.io.*;
import java.util.Scanner;
import java.util.StringTokenizer;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class lab1 {

    public static void main(String[] args) {
        String inputDir = "src/Lab1/input.txt";
        String outputDir = "src/Lab1/output.txt";
        parse(inputDir, outputDir);
    }

    static void parse(String inputDir, String outputDir) {

        System.out.print("S0 -> ");

        try (BufferedReader in = new BufferedReader(new FileReader(new File(inputDir)));
             BufferedWriter out = new BufferedWriter(new FileWriter(new File(outputDir)))) {
            String counterName = "";
            String limit = "";
            String counterOperation = "";

            System.out.print("S1 -> ");

            String line = in.readLine();

            System.out.print("S2 -> ");

            while (line != null) { // X1
                if (line.matches("^\\s*while.+")) { //X2
                    StringTokenizer tokenizer = new StringTokenizer(line, " ");

                    System.out.print("S4 -> ");

                    tokenizer.nextToken();

                    System.out.print("S5 -> ");

                    counterName = tokenizer.nextToken();
                    counterOperation = tokenizer.nextToken();
                    limit = tokenizer.nextToken();

                    System.out.print("S6 -> ");


                    switch (counterOperation) {
                        case "<": //X4
                            limit = limit +" - 1";

                            System.out.print("S7 -> ");

                        case "<=": //X5
                            counterOperation = "to";

                            System.out.print("S8 -> ");

                            break;
                        case ">": //X6
                            limit = limit +" + 1";

                            System.out.print("S9 -> ");

                        case ">=": //X7
                            counterOperation = "downto";

                            System.out.print("S10 -> ");

                            break;
                    }
                    out.write(String.format("  for %s %s %s do\n", counterName, counterOperation, limit));

                    System.out.print("S11 -> ");


                } else if (line.matches(String.format("^\\s*%s\\s*:=.*", counterName))) { //X3
                    //записать, шаг если надо
                } else {
                    out.write(String.format("%s\n", line));

                    System.out.print("S3 -> ");

                }
                line = in.readLine();

                System.out.print("S2 -> ");

            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        System.out.print("S0");

    }

}