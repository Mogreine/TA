package Lab2;

import java.io.*;
import java.util.StringTokenizer;

public class Lab2 {

    public static void main(String[] args) {
        System.out.println("S0");

        //S0

        String input = "";
        String res = "";

        try (BufferedReader in = new BufferedReader(new FileReader(new File("src/Lab2/input.txt")));
             BufferedWriter out = new BufferedWriter(new FileWriter(new File("src/Lab2/output.txt")))) {
            input = in.readLine();

            //S1

            System.out.print(String.format("S1 input = %s res = null\n", input));
            String validation = validation(input);
            if (validation != null) {
                out.write(validation);
            }
            else {
                if (input.substring(0, 2).equals("10")) {
                    res = Integer.toBinaryString(Integer.parseInt(input.substring(2, 34), 2) * Integer.parseInt(input.substring(34), 2));
                }
                else {
                    res = Integer.toBinaryString(Integer.parseInt(input.substring(2, 34), 2) / Integer.parseInt(input.substring(34), 2));
                }
                while (res.length() < 32) {
                    res = "0" + res;
                }
                out.write(res);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        System.out.print(String.format("S0 input = %s, res = %s", input, res));

        //S0

    }

    private static String validation(String str) {
        if (str.length() != 66) {
            return "Длина операндов неверная.";
        }
        if (str.substring(0, 2).compareTo("10") != 0 && str.substring(0, 2).compareTo("01") != 0) {
            return "Операторы введены неверно.";
        }
        if (!str.matches("^[0-1]+$")) {
            return "Операнды введены не в двоичном виде.";
        }
        return null;
    }


}
