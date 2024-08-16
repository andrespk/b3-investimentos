import { IsNotEmpty, IsNumber, IsOptional, Min } from 'class-validator';

export class ResgatarCdbRequestDto {
  @IsNumber({}, {message: "Deve ser um valor decimal." })
  @IsNotEmpty({message: "Deve ser maior que zero."  })
  @Min(0.01, {message: "Deve ser maior que zero."  })
  public valorInicial: number = 0;

  @IsNumber({}, {message: "Deve ser um valor inteiro." })
  @IsNotEmpty({message: "Deve ser maior que um mês."  })
  @Min(2, {message: "Deve ser maior que um mês."  })
  @IsNumber()
  public prazoEmMeses: number = 0;

  @IsOptional()
  @IsNumber({}, {message: "Deve ser um valor decimal." })
  @Min(0.01, {message: "Deve ser maior que zero."  })
  public percentualCdi?: number;

  @IsOptional()
  @IsNumber({}, {message: "Deve ser um valor decimal." })
  @Min(0.01, {message: "Deve ser maior que zero."  })
  public percentualCdiPagoPeloBanco?: number;
}
