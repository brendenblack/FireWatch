import { TradeExecutionDto } from "src/app/firewatch-api";

export class JournalEntry {
    constructor(date: moment.Moment) {
      this.date = date;
    }
  
    date: moment.Moment;
    symbols: JournalSymbol[] = [];
  
    /**
     * @description How many trades were executed on this date across all ticker symbols.
     */
    totalTrades(): number {
      return this.symbols
        .map(s => s.executions.length)
        .reduce((a, b) => a + b, 0);
    }
  
    /**
     * @description Returns the total volume for all trades on this date.
     */
    totalVolume(): number {
      return this.symbols
        .map(s => s.volume())
        .reduce((a, b) => a + b, 0);
    }
  
    /**
     * @description The sum of all fees that were paid for all trades made on this date.
     */
    totalFees(): number {
      return this.symbols
        .map(s => s.fees())
        .reduce((a, b) => a + b, 0);
    }

    /**
     * @description The sum of all commissions that were paid for all trades made on this date.
     */
    totalCommissions(): number {
      return this.symbols
        .map(s => s.commissions())
        .reduce((a, b) => a + b, 0);
    }

    totalProfitAndLoss(includeFeesAndCommissions: boolean = false): number {
      return this.symbols
        .map(s => s.profitAndLoss(includeFeesAndCommissions))
        .reduce((a, b) => a + b);
    }
  }

export class JournalSymbol {
    constructor(symbol: string, executions: TradeExecutionDto[]) {
      this.symbol = symbol;
      this.executions = executions.sort((a, b) => { return b.date.valueOf() - a.date.valueOf() }); 
    }
    symbol: string;
    executions: TradeExecutionDto[] = [];
  
    executionCount() {
      return this.executions.length;
    }
  
    volume(): number {
      let volume = 0;
  
      for (let execution of this.executions) {
        volume += Math.abs(execution.quantity);
      }
  
      return volume;
    }

    fees(): number {
      return this.executions
        .map(e => e.fees.amount)
        .reduce((a, b) => a + b);
    }

    commissions(): number {
      return this.executions
        .map(e => e.commissions.amount)
        .reduce((a, b) => a + b);
    }
  
    profitAndLoss(includeFeesAndCommissions: boolean = false): number {
      let pnl = 0;
      for (let execution of this.executions) {
        pnl += ((execution.unitPrice.amount * execution.quantity) * - 1);
        if (includeFeesAndCommissions) {
          // fees and commissions are negative values, so we add them to the total
          pnl += execution.fees.amount;
          pnl += execution.commissions.amount;
        }
      }
      return pnl;
    }
  }