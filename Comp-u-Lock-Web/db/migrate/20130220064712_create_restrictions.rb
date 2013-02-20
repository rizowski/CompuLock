class CreateRestrictions < ActiveRecord::Migration
  def change
    create_table :restrictions do |t|
    	t.references :account
      	t.timestamps
    end
  end
end
