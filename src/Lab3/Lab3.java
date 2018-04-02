package Lab3;

import java.util.HashMap;
import java.util.Stack;
import java.util.StringTokenizer;

public class Lab3 {

    public static class EquationParser {
        public String equation;
        private StringBuilder equationPA;

        public EquationParser(String equation) {
            this.equation = equation;
            equationPA = new StringBuilder();
        }

        public boolean parseEquation() {
            StringTokenizer elems = new StringTokenizer(equation, " ");
            String tmp;
            HashMap<String, Integer> priority = new HashMap<>();
            priority.put("*", 3);
            priority.put("/", 3);
            priority.put("+", 2);
            priority.put("-", 2);
            priority.put("(", 1);
            Stack<String> op = new Stack<>();
            while (elems.hasMoreTokens()) {
                tmp = elems.nextToken();
                if (tmp.charAt(0) >= '0' && tmp.charAt(0) <= 9) {
                    equationPA.append(tmp).append(" ");
                }
                else if (priority.containsKey(tmp)) {
                    if (op.empty() || priority.get(op.peek()) < priority.get(tmp)) {
                        op.push(tmp);
                    }
                    else if (priority.get(op.peek()) >= priority.get(tmp)) {
                        while (!op.empty() && priority.get(op.peek()) >= priority.get(tmp)) {
                            equationPA.append(op.pop()).append(" ");
                        }
                        op.push(tmp);
                    }
                    else if (tmp.equals("(")) {
                        op.push(tmp);
                    }
                    else {
                        while (!op.empty() && !op.peek().equals("(")) {
                            equationPA.append(op.pop()).append(" ");
                        }
                        if (op.empty()) {
                            return false;
                        }
                        else {
                            op.pop();
                        }
                    }
                }
            }
            return true;
        }

    }

    public static void main(String[] args) {
        EquationParser eq = new EquationParser("2 + 2)");
        System.out.print(eq .parseEquation());
    }

}
