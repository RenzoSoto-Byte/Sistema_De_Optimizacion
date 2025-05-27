SET SQL_SAFE_UPDATES = 0;
UPDATE inspectores
SET SueldoHora = 3
WHERE idInspectores = 2;
SET SQL_SAFE_UPDATES = 1;  -- Para volver a activar el modo seguro