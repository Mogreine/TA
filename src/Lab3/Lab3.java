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

        private boolean parseEquation() {
            StringTokenizer elems = new StringTokenizer(equation, " ");
            String tmp;
            int numbersCount = 0;
            int operationCount = 0;
            HashMap<String, Integer> priority = new HashMap<>();
            priority.put("*", 3);
            priority.put("/", 3);
            priority.put("+", 2);
            priority.put("-", 2);
            priority.put("(", 1);
            Stack<String> op = new Stack<>();
            boolean isCorrect = true;
            while (elems.hasMoreTokens() && isCorrect) {
                tmp = elems.nextToken();
                if (tmp.charAt(0) >= '0' && tmp.charAt(0) <= '9') {
                    if (Double.parseDouble(tmp) > 255.0) {
                        isCorrect = false;
                    }
                    else {
                        equationPA.append(tmp).append(" ");
                        numbersCount++;
                    }
                }
                else if (priority.containsKey(tmp)) {
                    if (tmp.equals("(")) {
                        op.push(tmp);
                    }
                    else if (op.empty() || priority.get(op.peek()) < priority.get(tmp)) {
                        op.push(tmp);
                        operationCount++;
                    }
                    else if (priority.get(op.peek()) >= priority.get(tmp)) {
                        while (!op.empty() && priority.get(op.peek()) >= priority.get(tmp)) {
                            equationPA.append(op.pop()).append(" ");
                        }
                        op.push(tmp);
                        operationCount++;
                    }
                }
                else if (tmp.equals(")")) {
                    while (!op.empty() && !op.peek().equals("(")) {
                        equationPA.append(op.pop()).append(" ");
                    }
                    if (op.empty()) {
                        isCorrect = false;
                    }
                    else {
                        op.pop();
                    }
                }
                else {
                    isCorrect = false;
                }
            }
            if (isCorrect) {
                isCorrect = numbersCount == operationCount + 1;
            }
            while (!op.empty() && isCorrect) {
                if (op.peek().equals("(")) {
                    isCorrect = false;
                }
                equationPA.append(op.pop()).append(" ");
            }
            return isCorrect;
        }

        public Double getResult() {
            Stack<Double> nums = new Stack<>();
            if (parseEquation()) {
                StringTokenizer elems = new StringTokenizer(equationPA.toString(), " ");
                String tmp;
                while (elems.hasMoreTokens()) {
                    tmp = elems.nextToken();
                    if (tmp.charAt(0) >= '0' && tmp.charAt(0) <= '9') {
                        nums.push(Double.parseDouble(tmp));
                    }
                    else {
                        switch (tmp) {
                            case "+":
                                nums.push(nums.pop() + nums.pop());
                                break;
                            case "-":
                                nums.push((nums.pop() - nums.pop()) * -1);
                                break;
                            case "*":
                                nums.push(nums.pop() * nums.pop());
                                break;
                            case "/":
                                double oper1 = nums.pop();
                                double oper2 = nums.pop();
                                nums.push(oper2 / oper1);
                                break;
                        }
                    }
                }
                return nums.pop();
            }
            else {
                return null;
            }
        }

    }

    public static void main(String[] args) {
        String equation = "255.5 - 2.5 * 2";
        EquationParser eq = new EquationParser(equation);
        System.out.print(eq.getResult());
    }

}
