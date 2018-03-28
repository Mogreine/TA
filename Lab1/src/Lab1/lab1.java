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
        try (BufferedReader in = new BufferedReader(new FileReader(new File(inputDir)));
             BufferedWriter out = new BufferedWriter(new FileWriter(new File(outputDir)))) {
            String counterName = "";
            String limit = "";
            String counterOperation = "";
            String line;
            while ((line = in.readLine()) != null) {
                if (line.matches("^\\s*while.+")) {
                    StringTokenizer tokenizer = new StringTokenizer(line, " ");
                    tokenizer.nextToken();
                    counterName = tokenizer.nextToken();
                    counterOperation = tokenizer.nextToken();
                    limit = tokenizer.nextToken();
                    switch (counterOperation) {
                        case "<":
                            limit = limit +" - 1";
                        case "<=":
                            counterOperation = "to";
                            break;
                        case ">":
                            limit = limit +" + 1";
                        case ">=":
                            counterOperation = "downto";
                            break;
                    }
                    out.write(String.format("  for %s %s %s do\n", counterName, counterOperation, limit));
                } else if (line.matches(String.format("^\\s*%s\\s*:=.*", counterName))) {
                    //записать, шаг если надо
                } else {
                    out.write(String.format("%s\n", line));
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

}
