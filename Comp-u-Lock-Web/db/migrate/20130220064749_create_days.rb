class CreateDays < ActiveRecord::Migration
  def change
    create_table :days do |t|
    	t.references :restriction
    	
      	t.timestamps
    end
  end
end
