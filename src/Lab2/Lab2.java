package Lab2;

import java.io.*;

public class Lab2 {

    public static void main(String[] args) {
        //S0

        try (BufferedReader in = new BufferedReader(new FileReader(new File("src/Lab2/input.txt")));
             BufferedWriter out = new BufferedWriter(new FileWriter(new File("src/Lab2/output.txt")))) {
            String input = in.readLine();

            //S1
            if (!validation(input)) {
                out.write("Данные введены некорректно");
            }
            else {
                String res = "";

                //S2

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

        //S0

    }

    private static boolean validation(String str) {
        if (str.length() != 66) {
            return false;
        }
        if (str.substring(0, 2).compareTo("10") != 0 && str.substring(0, 2).compareTo("01") != 0) {
            return false;
        }
        if (!str.matches("^[0-1]+$")) {
            return false;
        }
        return true;
    }


}
